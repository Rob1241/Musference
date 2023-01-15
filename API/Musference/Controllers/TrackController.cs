using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels.Track;
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
        [HttpGet("Search/{page}/{text}")]
        public ActionResult<IEnumerable<GetTrackDto>> SearchTrack([FromRoute] string text, [FromRoute] int page)
        {
            var tracklist = _service.SearchTrack(text, page);
            return Ok(tracklist);
        }
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<GetTrackDto>> GetAllTrackNewest([FromRoute] int page)
        {
            var response = _service.GetAllTrackNewest(page);
            return Ok(response);
        }
        [HttpGet("most_liked/{page}")]
        public ActionResult<IEnumerable<GetTrackDto>> GetAllTrackMostLiked([FromRoute] int page)
        {
            var response = _service.GetAllTrackMostLiked(page);
            return Ok(response);
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddTrack(AddTrackDto dto)
        {
            var id= _service.AddTrack(GetUserId(),dto);
            return Created($"api/Track/{id}",null);
        }
        [HttpDelete("{id}")]
        [Authorize]
        public ActionResult DeleteTrack([FromRoute]int id)
        {
            _service.DeleteTrack(id,GetUserId());
            return Ok();
        }
        [HttpPut("{id}/LikeTrack")]
        [Authorize]
        public ActionResult LikeTrack([FromRoute] int id)
        {
            _service.LikeTrack(id,GetUserId());
            return Ok();
        }
        //[HttpPut("{id}/UnLike")]
        ////[Authorize]
        //public ActionResult UnLikeTrack([FromRoute] int id)
        //{
        //    _service.UnLikeTrack(id);
        //    return Ok();
        //}
        protected int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    }
}
