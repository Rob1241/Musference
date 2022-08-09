using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Musference.Models.DTOs;
using Musference.Services;
using System.Security.Claims;

namespace Musference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TrackController : ControllerBase
    {
        private readonly ITrackService _service;

        public TrackController(ITrackService service)
        {
            _service = service;
        }
        [HttpGet]
        public ActionResult<IEnumerable<GetTrackDto>> GetAllTrack()
        {
            var tracklist = _service.GetAllTrack();
            return Ok(tracklist);
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddTrack(AddTrackDto dto)
        {
            int id= _service.AddTrack(GetUserId(),dto);
            return Created($"api/Track/{id}",null);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteTrack([FromRoute]int id)
        {
            _service.DeleteTrack(id);
            return Ok();
        }
        [HttpPut("{id}/LikeTrack")]
        [Authorize]
        public ActionResult LikeTrack([FromRoute] int id)
        {
            _service.LikeTrack(id);
            return Ok();
        }
        [HttpPut("{id}/UnLike")]
        [Authorize]
        public ActionResult UnLikeTrack([FromRoute] int id)
        {
            _service.UnLikeTrack(id);
            return Ok();
        }
        public int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    }
}
