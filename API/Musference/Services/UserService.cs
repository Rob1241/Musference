using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Musference.Data;
using Musference.Exceptions;
using Musference.Models;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Musference.Services
{
    public interface IUserService
    {
        public void Signup(SignupModel dto);
        public string GenerateJwtToken(LoginModel dto);
        public User ChangePassword(ChangePassword password, int id);
        public User ChangeName(ChangeName name, int id);
        public User ChangeDescription(ChangeDescription description, int id);
        public User ChangeEmail(ChangeEmail email,int id);
        public GetUserDto GetUser(int id);
        public IEnumerable<GetUserDto> GetAllUsers();
        public void Follow(int current_id, int user_to_follow_id);
        public void UnFollow(int current_id, int user_to_follow_id);
    }
    public class UserService: IUserService
    {
        private readonly AuthenticationModel _authenticationModel;
        private readonly DataBaseContext _context;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly IMapper _mapper;

        public UserService(DataBaseContext context, IPasswordHasher<User> passwordHasher, AuthenticationModel authenticationModel,IMapper mapper)
        {
            _authenticationModel = authenticationModel;
            _context = context;
            _passwordHasher = passwordHasher;
            _mapper = mapper;
        }
        public void Signup(SignupModel dto)
        {

            var user = new User()
            {
                Email = dto.Email,
                Login = dto.Login,
                Name = dto.Name,
                RoleId = dto.RoleId
            };
            var hashedPassword = _passwordHasher.HashPassword(user, dto.Password);
            user.HashedPassword = hashedPassword;
            _context.UsersDbSet.Add(user);
            _context.SaveChanges();
        }
        public string GenerateJwtToken(LoginModel dto)
        {
            var user = _context.UsersDbSet
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Email == dto.Login);
            if (user == null)
            {
                user = _context.UsersDbSet
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Login == dto.Login);
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
        public User ChangePassword(ChangePassword password, int id)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
            if(user == null)
            {
                throw new NotFoundException("User not found");
            }
            var hashedPassword = _passwordHasher.HashPassword(user, password.Password);
            user.HashedPassword = hashedPassword;
            _context.SaveChanges();
            return user;
        }
        public User ChangeName(ChangeName name, int id)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Name = name.Name;
            _context.SaveChanges();
            return user;
        }
        public User ChangeDescription(ChangeDescription description, int id)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Description = description.Description;
            _context.SaveChanges();
            return user;
        }
        public User ChangeEmail(ChangeEmail email, int id)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            user.Email = email.Email;
            _context.SaveChanges();
            return user;
        }
        public GetUserDto GetUser(int id)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == id);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            var getuserdto = _mapper.Map<GetUserDto>(user);
            return getuserdto;
        }
        public IEnumerable<GetUserDto> GetAllUsers()
        {
            var users = _context.UsersDbSet.ToList();
            var usersdto = new List<GetUserDto>();
            foreach(var item in users)
            {
                usersdto.Add(_mapper.Map<GetUserDto>(item));
            }
            return usersdto;
        }
        public void Follow(int user_to_follow_id, int current_user_id)
        {
            var user = _context.UsersDbSet
                .Include(u=>u.FollowedUsers)
                .FirstOrDefault(u => u.Id == current_user_id);
            var user_to_follow = _context.UsersDbSet
                .Include(_u => _u.FollowingUsers)
                .FirstOrDefault(u => u.Id == user_to_follow_id);
            if (user == null || user_to_follow == null)
            {
                throw new NotFoundException("User not found");
            }
            user.FollowedUsers.Add(user_to_follow);
            user_to_follow.FollowingUsers.Add(user);
            _context.SaveChanges();
        }
        public void UnFollow(int user_to_unfollow_id, int current_user_id)
        {
            var user = _context.UsersDbSet
                .Include(u => u.FollowedUsers)
                .FirstOrDefault(u => u.Id == current_user_id);
            var user_to_unfollow = _context.UsersDbSet
                .Include(_u => _u.FollowingUsers)
                .FirstOrDefault(u => u.Id == user_to_unfollow_id);
            if (user == null || user_to_unfollow == null)
            {
                throw new NotFoundException("User not found");
            }
            user.FollowedUsers.Remove(user_to_unfollow);
            user_to_unfollow.FollowingUsers.Remove(user);
            _context.SaveChanges();
        }
    }
}
