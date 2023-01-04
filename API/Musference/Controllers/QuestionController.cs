using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Musference.Models.DTOs;
using Musference.Models.Entities;
using Musference.Services;
using System.Security.Claims;

namespace Musference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        private readonly IQuestionService _service;

        public QuestionController(IQuestionService service)
        {
            _service = service;
        }
        [HttpPost]
        [Authorize]
        public ActionResult AddQuestion([FromBody] AddQuestionDTO questiondto)
        {
            var id = _service.AddQuestion(GetUserId(), questiondto);
            return Created($"/api/Question/{id}", null);
        }
        //[HttpGet]
        //public ActionResult<IEnumerable<GetQuestionDto>> GetAllQuestions()
        //{
        //    var list = _service.GetAllQuestions();
        //    return Ok(list);
        //}
        [HttpGet("{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllQuestionsNewest([FromRoute] int page)
        {
            var list = _service.GetAllQuestionsNewest(page);
            return Ok(list);
        }
        [HttpGet("most-liked/{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllQuestionsMostLiked([FromRoute] int page)
        {
            var list = _service.GetAllQuestionsMostLiked(page);
            return Ok(list);
        }
        [HttpGet("best-users/{page}")]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllQuestionsBestUsers([FromRoute] int page)
        {
            var list = _service.GetAllQuestionsBestUsers(page);
            return Ok(list);
        }
        [HttpGet("Search/{page}/{text}")]
        public ActionResult<IEnumerable<GetQuestionDto>> SearchQuestion([FromRoute] string text, [FromRoute] int page)
        {
            var list = _service.SearchQuestion(text, page);
            return Ok(list);
        }
        [HttpGet("OneQuestion/{id}/{page}")]
        public ActionResult GetQuestion([FromRoute] int id, [FromRoute] int page)
        {
            var question = _service.GetQuestion(id, page);
            return Ok(question);
        }
        [HttpPut("Plus/{id}")]
        [Authorize]
        public ActionResult PlusQuestion([FromRoute] int id)
        {
            _service.PlusQuestion(id, GetUserId());
            return Ok();
        }
        //[HttpPut("minus/{id}")]
        //[Authorize]
        //public ActionResult MinusQuestion([FromRoute] int id)
        //{
        //    _service.MinusQuestion(id, GetUserId());
        //    return Ok();
        //}
        [HttpPost("{id}/Answer")]
        [Authorize]
        public ActionResult AddAnswer([FromBody] AddAnswerDto dto, [FromRoute] int id)
        {
            var answer_id = _service.AddAnswer(dto, id, GetUserId());
            return Created($"api/Answer/{answer_id}", null);
        }
        [HttpPut("Answer/{id}/Plus")]
        [Authorize]
        public ActionResult PlusAnswer([FromRoute] int id)
        {
            _service.PlusAnswer(id, GetUserId());
            return Ok();
        }
        [HttpDelete("{id}/Delete)")]
        [Authorize]
        public ActionResult DeleteQuestion([FromRoute] int id)
        {
            _service.DeleteQuestion(id, GetUserId());
            return Ok();
        }
        [HttpDelete("Answer/{id}/Delete)")]
        [Authorize]
        public ActionResult DeleteAnswer([FromRoute] int id)
        {
            _service.DeleteAnswer(id, GetUserId());
            return Ok();
        }
        //[HttpPut("Answer/{id}/Minus")]
        //[Authorize]
        //public ActionResult MinusAnswer([FromRoute] int id)
        //{
        //    _service.MinusAnswer(id, GetUserId());
        //    return Ok();
        //}
        protected int GetUserId()
        {
            int id = Convert.ToInt32(User.FindFirstValue(ClaimTypes.NameIdentifier));
            return id;
        }
    
    }
}
