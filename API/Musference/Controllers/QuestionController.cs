using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Musference.Data;
using Musference.Models;
using Musference.Models.DTOs;
using Musference.Models.Mappers;
using AutoMapper;

namespace Musference.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class QuestionController : ControllerBase
    {
        public readonly DataBaseContext _context;
        public readonly IMapper _mapper;
        public QuestionController(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        [HttpPost]
        public ActionResult AddQuestion([FromBody] AddQuestionDTO questiondto)
        {
            var new_question = _mapper.Map<Question>(questiondto);
            new_question.DateAdded= DateTime.Now;
            _context.QuestionsDbSet.Add(new_question);
            _context.SaveChanges();
            return Created($"/api/Question/{new_question.Id}", null);
        }
        [HttpGet]
        public ActionResult<IEnumerable<Question>> GetAllQuestions()
        {
            List<Question> questionList = new List<Question>();
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            questionList = _context.QuestionsDbSet.ToList();
            foreach (var item in questionList)
            {
                var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == item.UserId);
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
               // getquestiondto.Username = user.Name;
                questionListDto.Add(getquestiondto);
            }
            return Ok(questionListDto);
        }
    }
}
