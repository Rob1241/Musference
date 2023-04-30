using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Musference.Data;
using Musference.Exceptions;
using Musference.Models;
using Musference.Models.DTOs;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Musference.Logic;
using Musference.Models.EndpointModels.User;
using Musference.Models.Entities;
using static System.Net.WebRequestMethods;

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
        public void ChangeImage(ChangeImage changeImage, int id);
        public Task<GetUserDto> GetUser(int id);
        public Task<UsersResponse> SearchUsers(string text, int page);
        public Task<UsersResponse> GetAllUsersReputation(int page);
        public Task<UsersResponse> GetAllUsersNewest(int page);
        public void ChangeCity(ChangeCity city, int id);
        public void ChangeContact(ChangeContact contact, int id);
        public void ChangeCountry(ChangeCountry country, int id);
        public void DeleteUser(int id, DeleteUser model);
    }
    public class UserService: IUserService
    {
        private readonly AuthenticationModel _authenticationModel;
        private readonly DataBaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;
        private readonly IPagination _pagination;

        public UserService(DataBaseContext context, IPasswordHasher<User> passwordHasher, AuthenticationModel authenticationModel,
            IMapper mapper,IPagination pagination)
        {
            _authenticationModel = authenticationModel;
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
            _pagination = pagination;
        }
        public async void Signup(SignupModel dto)
        {
                if (await _context.UsersDbSet.AnyAsync(u => u.Email == dto.Email))
                {
                    throw new Exception("Email already exists");
                }
                if (await _context.UsersDbSet.AnyAsync(u => u.Login == dto.Login))
                {
                    throw new Exception("Login already exists");
                }
                if (await _context.UsersDbSet.AnyAsync(u => u.Name == dto.Name))
                {
                    throw new Exception("Name already exists");
                }
                if (dto.Password != dto.ConfirmPassword)
                {
                    throw new Exception("Password must be the same as the confirm password");
                }
                var user = new User()
                {
                    Email = dto.Email,
                    Login = dto.Login,
                    Name = dto.Name,
                    DateAdded = DateTime.Now,
                    ImageFile = "https://cdn.pixabay.com/photo/2012/04/18/00/07/silhouette-of-a-man-36181_960_720.png"
                };
                var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
                user.HashedPassword = hashedPassword;
                await _context.UsersDbSet.AddAsync(user);
                await _context.SaveChangesAsync();
        }
        public async Task<string> GenerateJwtToken(LoginModel dto)
        {
            var user = await _context.UsersDbSet
                .FirstOrDefaultAsync(u => u.Email == dto.Login);
            if (user == null)
            {
                user = await _context.UsersDbSet
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
            if(await _context.UsersDbSet.AnyAsync(u => u.Email == email.Email)&&user.Email!=email.Email)
                throw new NotFoundException("Email already exists");
            user.Email = email.Email;
            await _context.SaveChangesAsync();
        }
        public async void ChangeImage(ChangeImage changeImage, int id)
        {
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.ImageFile = changeImage.ImageFile;
            await _context.SaveChangesAsync();
        }
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
            var pageResults = 30f;
            var pageCount = Math.Ceiling(_context.UsersDbSet.Count() / pageResults);
            var sortedusers = await _context.UsersDbSet.OrderByDescending(u => u.Reputation).ToListAsync();
            var response = _pagination.UserPagination(sortedusers, pageResults,page, pageCount);
            return response;
        }
        public async Task<UsersResponse> GetAllUsersNewest(int page)
        {
            var pageResults = 12f;
            var pageCount = Math.Ceiling(_context.UsersDbSet.Count() / pageResults);
            var sortedusers = await _context.UsersDbSet.OrderByDescending(u => u.DateAdded).ToListAsync();
            var response = _pagination.UserPagination(sortedusers, pageResults, page, pageCount);
            return response;
        }
        public async Task<UsersResponse> SearchUsers(string text, int page)
        {
            List<GetUserDto> userListDto = new List<GetUserDto>();
            var pageResults = 30f;
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
        public async void DeleteUser(int id, DeleteUser model) 
        {
            var user = await _context.UsersDbSet
                .Include(u=>u.Tracks)
                .Include(u => u.Questions)
                .Include(u => u.Answers)
                .FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var result = _passwordHasher.VerifyHashedPassword(user, user.HashedPassword, model.password);
            if (result == PasswordVerificationResult.Failed)
                throw new UnauthorizedException("Wrong password");
            if (user.Tracks.Count() > 0)
            {
                for (int i = 0; i < user.Tracks.Count(); i++)
                {
                    int trackid = user.Tracks[i].Id;
                    var track = _context.TracksDbSet.FirstOrDefault(t => t.Id == trackid);
                    _context.TracksDbSet.Remove(track);
                }
            }
            if (user.Questions.Count() > 0)
            {
                for (int i = 0; i < user.Questions.Count(); i++)
                {
                    int questionid = user.Questions[i].Id;
                    var question = _context.QuestionsDbSet.FirstOrDefault(t => t.Id == questionid);
                    _context.QuestionsDbSet.Remove(question);
                }
            }
            if (user.Answers.Count() > 0)
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
    }
}
