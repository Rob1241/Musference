using AutoMapper;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Musference.Data;
using Musference.Exceptions;
using Musference.Models;
using Musference.Models.DTOs;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Musference.Logic;
using Musference.Models.EndpointModels.User;
using Musference.Models.Entities;

namespace Musference.Services
{
    public interface IUserService
    {
        public void Signup(SignupModel dto);
        public Task<string> GenerateJwtToken(LoginModel dto);
        public void ChangePassword(ChangePassword password, int id);
        public void ChangeName(ChangeName name, int id);
        public void ChangeDescription(ChangeDescription description, int id);
        public void ChangeEmail(ChangeEmail email,int id);
        public Task<GetUserDto> GetUser(int id);
        public Task<UsersResponse> SearchUsers(string text, int page);
        public Task<UsersResponse> GetAllUsersReputation(int page);
        public Task<UsersResponse> GetAllUsersNewest(int page);
        public void ChangeCity(ChangeCity city, int id);
        public void ChangeContact(ChangeContact contact, int id);
        public void ChangeCountry(ChangeCountry country, int id);
        //public Task<int> ReportUser(int id, int userId, ReportModel model);
        //public Task<ReportedUsersResponse> GetReportedUsers(int page);
        public void DeleteUser(int id, DeleteUser model);
        //public void DeleteUserAdmin(DeleteUserAdmin model);
        //public void ResetPassword(ResetPasswordModel model);
        //public void SendResetCode(PasswordResetEmail mail);
    }
    public class UserService: IUserService
    {
        private readonly AuthenticationModel _authenticationModel;
        private readonly DataBaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        //private readonly SendGridKey _sgKey;
        private readonly ICloudinaryhandler _cloudinary;
        private readonly IPagination _pagination;

        public UserService(DataBaseContext context, IPasswordHasher<User> passwordHasher, AuthenticationModel authenticationModel,IMapper mapper, ICloudinaryhandler cloudinary,IPagination pagination)
        {
            _authenticationModel = authenticationModel;
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _cloudinary = cloudinary;
            _pagination = pagination;
        }
        public async void Signup(SignupModel dto)
        {
            //byte[] newSalt = new byte[128 / 8];

            //using (var rngCsp = new RNGCryptoServiceProvider())
            //{
            //    rngCsp.GetNonZeroBytes(newSalt);
            //}
            if(await _context.UsersDbSet.AnyAsync(u => u.Email == dto.Email)){
                throw new NotFoundException("Email already exists");
            }
            if (await _context.UsersDbSet.AnyAsync(u => u.Login == dto.Login)){
                throw new NotFoundException("Login already exists");
            }
            if (await _context.UsersDbSet.AnyAsync(u => u.Name == dto.Name)){
                throw new NotFoundException("Name already exists");
            }
            var user = new User()
            {
                Email = dto.Email,
                Login = dto.Login,
                Name = dto.Name,
                DateAdded = DateTime.Now,
                RoleId = 1
                //Salt = newSalt
            };
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.HashedPassword = hashedPassword;
            await _context.UsersDbSet.AddAsync(user);
            await _context.SaveChangesAsync();
        }
        public async Task<string> GenerateJwtToken(LoginModel dto)
        {
            var user = await _context.UsersDbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Email == dto.Login);
            if (user == null)
            {
                user = await _context.UsersDbSet
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Login == dto.Login);
            }
            if (user == null)
                throw new UnauthorizedException("Wrong login/email or password");
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, dto.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Wrong login/email or password");

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                new Claim(ClaimTypes.Name,$"{user.Name}"),
                new Claim(ClaimTypes.Role,$"{user.Role}"),
                new Claim("Reputation",user.Reputation.ToString())
            };

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_authenticationModel.JwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiretime = DateTime.Now.AddHours(_authenticationModel.JwtExpireHours);

            var token = new JwtSecurityToken(_authenticationModel.JwtIssuer,
                _authenticationModel.JwtIssuer,
                claims,
                expires: expiretime,
                signingCredentials: credentials);

            var tokenHandler = new JwtSecurityTokenHandler();
            return tokenHandler.WriteToken(token);
        }
        //public async void ResetPassword(ResetPasswordModel model)
        //{
        //    var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Email == model.Email);
        //    var modelfromdb = await _context.PasswordResetDbSet.FirstOrDefaultAsync(m=>m.UserId==user.Id);
        //    string HashCode = Hash(model.ResetCode, user.Salt);
        //    if (HashCode == modelfromdb.HashedCode)
        //    {
        //        user.HashedPassword = _passwordHasher.HashPassword(user, model.NewPassword);
        //    }
        //}
        //public async void SendResetCode([FromBody] PasswordResetEmail mail)
        //{
        //    var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Email == mail.Email);
        //    if (user == null)
        //    {
        //        throw new NotFoundException("E-mail doesn't exist");
        //    }
        //    string ResetCode = CreateCode();
        //    ResetCodeSend(mail.Email, ResetCode);
        //    string HashedCode = Hash(ResetCode, user.Salt);
        //    var NewCode = new ResetCodeModel
        //    {
        //        HashedCode = HashedCode,
        //        Expiration = DateTime.Now.AddHours(1),
        //        UserId = user.Id
        //    };
        //    await _context.PasswordResetDbSet.AddAsync(NewCode);
        //    await _context.SaveChangesAsync();

        //}
        public async void ChangePassword(ChangePassword password, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, password.OldPassword);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Wrong password");
            var hashedPassword = _passwordHasher.HashPassword(user, password.Password);
            user.HashedPassword = hashedPassword;
            await _context.SaveChangesAsync();
        }
        public async void ChangeName(ChangeName name, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Name = name.Name;
            await _context.SaveChangesAsync();
        }
        public async void ChangeDescription(ChangeDescription description, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Description = description.Description;
            await _context.SaveChangesAsync();
        }
        public async void ChangeCity(ChangeCity city, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.City = city.City;
            await _context.SaveChangesAsync();
        }
        public async void ChangeContact(ChangeContact contact, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Contact = contact.Contact;
            await _context.SaveChangesAsync();
        }
        public async void ChangeCountry(ChangeCountry country, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Country = country.Country;
            await _context.SaveChangesAsync();
        }
        public async void ChangeEmail(ChangeEmail email, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if(await _context.UsersDbSet.AnyAsync(u => u.Email == email.Email))
                throw new NotFoundException("Email already exists");
            user.Email = email.Email;
            await _context.SaveChangesAsync();
        }
        //public void ChangeImage(ChangeImage changeImage, int id)
        //{
        //    var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
        //    if (user == null)
        //    {
        //        throw new NotFoundException("User not found");
        //    }
        //    user.ImageFile = _cloudinary.GetFileStringAndUpload(changeImage.Image,"image").Result;
        //    _context.SaveChanges();
        //}
        public async Task<GetUserDto> GetUser(int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var getuserdto = _mapper.Map<GetUserDto>(user);
            return getuserdto;
        }
        public async Task<UsersResponse> GetAllUsersReputation(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.UsersDbSet.Count() / pageResults);
            var sortedusers = await _context.UsersDbSet.OrderByDescending(u => u.Reputation).ToListAsync();
            var response = _pagination.UserPagination(sortedusers, pageResults,page, pageCount);
            return response;
        }
        public async Task<UsersResponse> GetAllUsersNewest(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.UsersDbSet.Count() / pageResults);
            var sortedusers = await _context.UsersDbSet.OrderBy(u => u.DateAdded).ToListAsync();
            var response = _pagination.UserPagination(sortedusers, pageResults, page, pageCount);
            return response;
        }
        public async Task<UsersResponse> SearchUsers(string text, int page)
        {
            List<GetUserDto> userListDto = new List<GetUserDto>();
            var pageResults = 10f;
            var usersList = await _context.UsersDbSet
                                        .Where(u => (u.Name.ToLower().Contains(text.ToLower())))
                                        .ToListAsync();
            if (usersList == null)
            {
                throw new NotFoundException("Users not found");
            }
            var pageCount = Math.Ceiling(usersList.Count() / pageResults);
            var response = _pagination.UserPagination(usersList, pageResults, page, pageCount);
            return response;
        }
        //public async Task<ReportedUsersResponse> GetReportedUsers(int page)
        //{
        //    var pageResults = 10f;
        //    var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
        //    var users = await _context.ReportedUsersDbSet
        //        .Skip((page - 1) * (int)pageResults)
        //        .Take((int)pageResults)
        //        .ToListAsync();
        //    var response = new ReportedUsersResponse
        //    {
        //        ReportedUsers = users,
        //        CurrentPage = page,
        //        Pages = (int)pageCount
        //    };
        //    return response;
        //}
        //public async Task<int> ReportUser(int id, int userId, ReportModel Model)
        //{
        //    var usertoreport = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
        //    if (usertoreport == null)
        //    {
        //        throw new NotFoundException("User to report not found");
        //    }
        //    var reportinguser = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
        //    if (reportinguser == null)
        //    {
        //        throw new NotFoundException("Reporting user not found");
        //    }
        //    string reason = Model.reason;
        //    var new_report = new ReportedUser()
        //    {
        //        ReporteddUser = usertoreport,
        //        UserThatReported = reportinguser,
        //        reason = reason
        //    };
        //    await _context.ReportedUsersDbSet.AddAsync(new_report);
        //    await _context.SaveChangesAsync();
        //    return new_report.Id;
        //}
        public async void DeleteUser(int id, DeleteUser model) 
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, model.Password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Wrong password");
            //trzeba usunac pytania, utwory, odpowiedzi 
            //nie mzona usuwac tracksliked bo wtedy usune wszystkie tracki ktore gosciu
            //polubil
            if (user.Tracks.Count() > 0)
            {
                for (int i = 0; i < user.Tracks.Count(); i++)
                {
                    int trackid = user.Tracks[i].Id;
                    var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == trackid);
                    _context.TracksDbSet.Remove(track);
                }
            }
            if (user.Tracks.Count() > 0)
            {
                for (int i = 0; i < user.Questions.Count(); i++)
                {
                    int questionid = user.Questions[i].Id;
                    var question = _context.QuestionsDbSet.FirstOrDefault(t => t.Id == questionid);
                    _context.QuestionsDbSet.Remove(question);
                }
            }
            if (user.Tracks.Count() > 0)
            {
                for (int i = 0; i < user.Answers.Count(); i++)
                {
                    int answerid = user.Answers[i].Id;
                    var answer = _context.AnswersDbSet.FirstOrDefault(t => t.Id == answerid);
                    _context.AnswersDbSet.Remove(answer);
                }
            }
            _context.UsersDbSet.Remove(user);
            await _context.SaveChangesAsync();
        }
        //public async void DeleteUserAdmin(DeleteUserAdmin model)
        //{
        //        var user =await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Name == model.Name);
        //        if (user == null)
        //        {
        //            throw new NotFoundException("User not found");
        //        }
        //        if (user.Tracks.Count() > 0)
        //        {
        //            for (int i = 0; i < user.Tracks.Count(); i++)
        //            {
        //                int trackid = user.Tracks[i].Id;
        //                var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == trackid);
        //                _context.TracksDbSet.Remove(track);
        //            }
        //        }
        //        if (user.Tracks.Count() > 0)
        //        {
        //            for (int i = 0; i < user.Questions.Count(); i++)
        //            {
        //                int questionid = user.Questions[i].Id;
        //                var question = _context.QuestionsDbSet.FirstOrDefault(t => t.Id == questionid);
        //                _context.QuestionsDbSet.Remove(question);
        //            }
        //        }
        //        if (user.Tracks.Count() > 0)
        //        {
        //            for (int i = 0; i < user.Answers.Count(); i++)
        //            {
        //                int answerid = user.Answers[i].Id;
        //                var answer = _context.AnswersDbSet.FirstOrDefault(t => t.Id == answerid);
        //                _context.AnswersDbSet.Remove(answer);
        //            }
        //        }
        //        _context.UsersDbSet.Remove(user);
        //        await _context.SaveChangesAsync();
        //}
        //private string CreateCode()
        //{
        //    char[] chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890".ToCharArray();
        //    int size = 6;
        //    byte[] data = new byte[4 * size];
        //    using (var crypto = RandomNumberGenerator.Create())
        //    {
        //        crypto.GetBytes(data);
        //    }
        //    StringBuilder token = new StringBuilder(size);
        //    for (int i = 0; i < size; i++)
        //    {
        //        var rnd = BitConverter.ToUInt32(data, i * 4);
        //        var idx = rnd % chars.Length;

        //        token.Append(chars[idx]);
        //    }
        //    return token.ToString();
        //}
        //private void ResetCodeSend(string UserEmail, string ResetCode)
        //{
        //    var client = new SendGridClient(_sgKey.API);
        //    var msg = new SendGridMessage()
        //    {
        //        Subject = "Here is your password reset code",
        //        HtmlContent = ResetCode
        //    };
        //    msg.From = new EmailAddress("musference@gmail.com", "Musference");
        //    msg.AddTo(UserEmail);
        //    client.SendEmailAsync(msg).ConfigureAwait(false);

        //}
        //private string Hash(string password, byte[] salt)
        //{
        //    string hashed = Convert.ToBase64String(KeyDerivation.Pbkdf2(
        //                    password: password,
        //                    salt: salt,
        //                    prf: KeyDerivationPrf.HMACSHA512,
        //                    iterationCount: 100000,
        //                    numBytesRequested: 256 / 8));
        //    return hashed;
        //}
    }
}
