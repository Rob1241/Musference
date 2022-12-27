﻿using Microsoft.AspNetCore.Authorization;
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
        [HttpGet("Search")]
        public ActionResult<IEnumerable<GetTrackDto>> SearchTrack([FromBody] string text)
        {
            var tracklist = _service.SearchTrack(text);
            return Ok(tracklist);
        }
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<GetTrackDto>> GetAllTrackNewest([FromRoute] int page)
        {
            var response = _service.GetAllTrackNewest(page);
            return Ok(response);
        }
        [HttpGet("best_users/{id}")]
        public ActionResult<IEnumerable<GetTrackDto>> GetAllTrackBestUsers([FromRoute] int page)
        {
            var response = _service.GetAllTrackBestUsers(page);
            return Ok(response);
        }
        [HttpPost]
        //[Authorize]
        public ActionResult AddTrack(AddTrackDto dto)
        {
            var id= _service.AddTrack(GetUserId(),dto);
            return Created($"api/Track/{id}",null);
        }
        [HttpDelete("{id}")]
        //[Authorize]
        public ActionResult DeleteTrack([FromRoute]int id)
        {
            _service.DeleteTrack(id,GetUserId());
            return Ok();
        }
        [HttpPut("{id}/LikeTrack")]
        //[Authorize]
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
