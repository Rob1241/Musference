using Microsoft.AspNetCore.Mvc;
using Musference.Models;
using Musference.Services;
using Musference.Models.DTOs;
using Microsoft.AspNetCore.Authorization;

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
            var id = _service.AddQuestion(questiondto);
            return Created($"/api/Question/{id}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<GetQuestionDto>> GetAllQuestions()
        {
            var list = _service.GetAllQuestions();
            return Ok(list);
        }
        [HttpGet("{id}")]
        public ActionResult GetQuestion([FromRoute] int id)
        {
            var question = _service.GetQuestion(id);
            return Ok(question);
        }

        [HttpPut("plus/{id}")]
        [Authorize]
        public ActionResult PlusQuestion([FromRoute] int id)
        {
            _service.PlusQuestion(id);
            return Ok();
        }
        [HttpPut("minus/{id}")]
        [Authorize]
        public ActionResult MinusQuestion([FromRoute] int id)
        {
            _service.MinusQuestion(id);
            return Ok();
        }
        [HttpPost("{id}/Answer")]
        [Authorize]
        public ActionResult AddAnswer([FromBody] AddAnswerDto dto, [FromRoute] int id)
        {
            int answer_id = _service.AddAnswer(dto, id);
            return Created($"api/Answer/{answer_id}",null);
        }
        [HttpPut("Answer/{id}/Plus")]
        [Authorize]
        public ActionResult PlusAnswer([FromRoute] int id)
        {
            _service.PlusAnswer(id);
            return Ok();
        }
        [HttpPut("Answer/{id}/Minus")]
        [Authorize]
        public ActionResult MinusAnswer([FromRoute] int id)
        {
            _service.MinusAnswer(id);
            return Ok();
        }

    }
}
