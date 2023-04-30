using Microsoft.AspNetCore.Mvc;
using Musference.Services;
using Musference.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Musference.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Musference.Models.EndpointModels.User;
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
        [HttpGet("newest/{page}")]
        public ActionResult<IEnumerable<GetUserDto>> GetAllUsersNewest([FromRoute] int page)
        {
            var response = _service.GetAllUsersNewest(page);
            return Ok(response);
        }
        [HttpGet("reputation/{page}")]
        public ActionResult<IEnumerable<GetUserDto>> GetAllUsersReputation([FromRoute] int page)
        {
            var response = _service.GetAllUsersReputation(page);
            return Ok(response);
        }

        [HttpGet("search/{page}/{text}")]
        public ActionResult<IEnumerable<GetUserDto>> SearchUsers([FromRoute] string text,[FromRoute] int page)
        {
            var users = _service.SearchUsers(text, page);
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
            _service.ChangePassword(password, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeName")]
        [Authorize]
        public ActionResult ChangeName([FromBody] ChangeName name)
        {
            _service.ChangeName(name, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeDescription")]
        [Authorize]
        public ActionResult ChangeDescription([FromBody] ChangeDescription description)
        {
            _service.ChangeDescription(description, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeCity")]
        [Authorize]
        public ActionResult ChangeCity([FromBody] ChangeCity city)
        {
            _service.ChangeCity(city, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeCountry")]
        [Authorize]
        public ActionResult ChangeCountry([FromBody] ChangeCountry country)
        {
            _service.ChangeCountry(country, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeContact")]
        [Authorize]
        public ActionResult ChangeContact([FromBody] ChangeContact contact)
        {
            _service.ChangeContact(contact, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeEmail")]
        [Authorize]
        public ActionResult ChangeEmail([FromBody] ChangeEmail email)
        {
            _service.ChangeEmail(email, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeImage")]
        [Authorize]
        public ActionResult ChangeImage([FromBody] ChangeImage image)
        {
            _service.ChangeImage(image, GetUserId());
            return Ok();
        }
        [HttpDelete]
        [Authorize]
        public ActionResult DeleteUser([FromBody] DeleteUser model) 
        {
            _service.DeleteUser(GetUserId(),model);
            return Ok();
        }
        protected int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    }
}
