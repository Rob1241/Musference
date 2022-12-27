using Microsoft.AspNetCore.Mvc;
using Musference.Services;
using Musference.Models.EndpointModels;
using Musference.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using Musference.Exceptions;
using Musference.Models;
using Microsoft.AspNetCore.Authorization.Infrastructure;

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
        //[HttpGet]
        //public ActionResult<IEnumerable<GetUserDto>> GetAllUsers()
        //{
        //    var users = _service.GetAllUsers();
        //    return Ok(users);
        //}
        //[HttpGet("{page}")]
        //public ActionResult<IEnumerable<GetUserDto>> GetAllUsers([FromRoute] int page)
        //{
        //    var response = _service.GetAllUsers(page);
        //    return Ok(response);
        //}
        //[HttpPost("SendResetCode")]
        //public ActionResult SendResetCode([FromBody]PasswordResetEmail mail)
        //{
        //    _service.SendResetCode(mail);
        //    return Ok();
        //}
        //[HttpPost("ResetPassword")]
        //public ActionResult ResetPassword([FromBody]ResetPasswordModel model)
        //{
        //    _service.ResetPassword(model);
        //    return Ok();
        //}
        [HttpGet("newest/{page}")]
        public ActionResult<IEnumerable<GetUserDto>> GetAllUsersNewest([FromRoute] int page)
        {
            var response = _service.GetAllUsersNewest(page);
            return Ok(response);
        }
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<GetUserDto>> GetAllUsersReputation([FromRoute] int page)
        {
            var response = _service.GetAllUsersReputation(page);
            return Ok(response);
        }
        //[Authorize(Roles="Admin")]
        [HttpGet("reported_users/{page}")]
        public ActionResult<IEnumerable<ReportedUser>> GetReportedUsers([FromRoute] int page)
        {
            var response = _service.GetReportedUsers(page);
            return Ok(response);
        }

        [HttpGet("search")]
        public ActionResult<IEnumerable<GetUserDto>> SearchUsers([FromBody] string text)
        {
            var users = _service.SearchUsers(text);
            return Ok(users);
        }
        [HttpGet("{id}")]
        public ActionResult GetUser([FromRoute] int id)
        {
            var user = _service.GetUser(id);
            return Ok(user);
        }
        [HttpPost("{userid}/report")]
        public ActionResult ReportUser([FromRoute] int userid, [FromBody] ReportModel model)
        {
            var id = _service.ReportUser(userid,GetUserId(),model);
            return Created($"/api/ReportedUsers/{id}", null);
        }
        [HttpPost("Signup")]
        public ActionResult Signup([FromBody] SignupModel dto)
        {
            _service.Signup(dto);
            return Ok();
        }
        [HttpPut("ChangePassword")]
        //[Authorize]
        public ActionResult ChangePassword([FromBody] ChangePassword password)
        {
            _service.ChangePassword(password, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeName")]
        //[Authorize]
        public ActionResult ChangeName([FromBody] ChangeName name)
        {
            _service.ChangeName(name, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeDescription")]
        //[Authorize]
        public ActionResult ChangeDescription([FromBody] ChangeDescription description)
        {
            _service.ChangeDescription(description, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeCity")]
        //[Authorize]
        public ActionResult ChangeCity([FromBody] ChangeCity city)
        {
            _service.ChangeCity(city, GetUserId());
            return Ok();
        }
        [HttpPut("DeleteUser")]
        //[Authorize]
        public ActionResult DeleteUser(int id, [FromBody] DeleteUser model)
        {
            _service.DeleteUser(GetUserId(),model);
            return Ok();
        }
        [HttpPut("DeleteUserAdmin")]
        //[Authorize(Roles="Admin")]
        public ActionResult DeleteUserAdmin([FromBody] DeleteUserAdmin model)
        {
            _service.DeleteUserAdmin(model);
            return Ok();
        }
        [HttpPut("ChangeCountry")]
        //[Authorize]
        public ActionResult ChangeCountry([FromBody] ChangeCountry country)
        {
            _service.ChangeCountry(country, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeContact")]
        //[Authorize]
        public ActionResult ChangeContact([FromBody] ChangeContact contact)
        {
            _service.ChangeContact(contact, GetUserId());
            return Ok();
        }
        [HttpPut("ChangeEmail")]
        //[Authorize]
        public ActionResult ChangeEmail([FromBody] ChangeEmail email)
        {
            _service.ChangeEmail(email, GetUserId());
            return Ok();
        }
        [HttpDelete("Delete")]
        //[Authorize]
        public ActionResult DeletePassword([FromBody] DeleteUser model) 
        {
            _service.DeleteUser(GetUserId(),model);
            return Ok();
        }
        //[HttpPut("{id}/Follow")]
        //[Authorize]
        //public ActionResult Follow([FromRoute] int id)
        //{
        //    _service.Follow(id, GetUserId());
        //    return Ok();
        //}
        //[HttpPut("{id}/UnFollow")]
        //[Authorize]
        //public ActionResult UnFollow([FromRoute] int id)
        //{
        //    _service.UnFollow(id, GetUserId());
        //    return Ok();
        //}

        protected int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    }
}
