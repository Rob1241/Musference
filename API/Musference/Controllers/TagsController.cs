using Microsoft.AspNetCore.Mvc;
using Musference.Models;
using Musference.Services;
using Musference.Models.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace Musference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TagsController : ControllerBase
    {
        private readonly ITagsService _service;
        public TagsController(ITagsService service)
        {
            _service = service;
        }
        //[HttpPost]
        //[Authorize]
        //public ActionResult AddQuestion([FromBody] AddQuestionDTO questiondto)
        //{
        //    var id = _service.AddQuestion(GetUserId(), questiondto);
        //    return Created($"/api/Question/{id}", null);
        //}

        [HttpGet("track/{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllTagsMostPopularTrack([FromRoute] int page)
        {
            var list = _service.GetAllTagsMostPopularTracks(page);
            return Ok(list);
        }
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllTagsMostPopularQuestion([FromRoute] int page)
        {
            var list = _service.GetAllTagsMostPopularQuestion(page);
            return Ok(list);
        }
        [HttpGet("name/{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllTagsName([FromRoute] int page)
        {
            var list = _service.GetAllTagsName(page);
            return Ok(list);
        }
        [HttpGet("Search")]
        public ActionResult<IEnumerable<GetTagDto>> SearchTag([FromBody] string text)
        {
            var list = _service.SearchTag(text);
            return Ok(list);
        }
    }
}
