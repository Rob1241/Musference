using Microsoft.AspNetCore.Mvc;
using Musference.Services;
using Musference.Models.EndpointModels;
using Musference.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Musference.Exceptions;

namespace Musference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _service;

        public UserController(IUserService service)
        {
            _service = service;
        }
        [HttpPost("Login")]
        public ActionResult Login([FromBody] LoginModel dto)
        {
            var token = _service.GenerateJwtToken(dto);
            return Ok(token);
        }
        [HttpGet]
        public ActionResult<IEnumerable<GetUserDto>> GetAllUsers()
        {
            var users = _service.GetAllUsers();
            return Ok(users);
        }
        [HttpGet("{id}")]
        public ActionResult GetUser([FromRoute] int id)
        {
            var user = _service.GetUser(id);
            return Ok(user);
        }

        [HttpPost("Signup")]
        public ActionResult Signup([FromBody] SignupModel dto)
        {
            _service.Signup(dto);
            return Ok();
        }
        [HttpPut("ChangePassword")]
        [Authorize]
        public ActionResult ChangePassword([FromBody] ChangePassword password)
        {
            var user = _service.ChangePassword(password, GetUserId());
            return Ok(user);
        }
        [HttpPut("ChangeName")]
        [Authorize]
        public ActionResult ChangeName([FromBody] ChangeName name)
        {
            var user = _service.ChangeName(name, GetUserId());
            return Ok(user);
        }
        [HttpPut("ChangeDescription")]
        [Authorize]
        public ActionResult ChangeDescription([FromBody] ChangeDescription description)
        {
            var user = _service.ChangeDescription(description, GetUserId());
            return Ok(user);
        }
        [HttpPut("ChangeEmail")]
        [Authorize]
        public ActionResult ChangeEmail([FromBody] ChangeEmail email)
        {
            var user = _service.ChangeEmail(email, GetUserId());
            return Ok(user);
        }
        [HttpPut("{id}/Follow")]
        [Authorize]
        public ActionResult Follow([FromRoute] int id)
        {
            _service.Follow(id, GetUserId());
            return Ok();
        }
        [HttpPut("{id}/UnFollow")]
        [Authorize]
        public ActionResult UnFollow([FromRoute] int id)
        {
            _service.UnFollow(id, GetUserId());
            return Ok();
        }

        public int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    }
}
