using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Musference.Data;
using Musference.Models;
using Musference.Models.DTOs;

namespace Musference.Services
{
    public interface IQuestionService
    {
        public int AddQuestion(AddQuestionDTO questiondto);
        public IEnumerable<GetQuestionDto> GetAllQuestions();
        public Question GetQuestion(int id);
        public void PlusQuestion(int id);
        public void MinusQuestion(int id);
        public int AddAnswer(AddAnswerDto dto, int id);
        public void PlusAnswer(int id);
        public void MinusAnswer(int id);
    }
    public class QuestionService : IQuestionService
    {
        public readonly DataBaseContext _context;
        public readonly IMapper _mapper;
        public QuestionService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public int AddQuestion(AddQuestionDTO questiondto)
        {
            var new_question = _mapper.Map<Question>(questiondto);
            new_question.DateAdded = DateTime.Now;
            _context.QuestionsDbSet.Add(new_question);
            _context.SaveChanges();
            return new_question.Id;
        }
        public IEnumerable<GetQuestionDto> GetAllQuestions()
        {
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            var questionList = _context.QuestionsDbSet.ToList();
            foreach (var item in questionList)
            {
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
                questionListDto.Add(getquestiondto);
            }
            return questionListDto;
        }
        public Question GetQuestion(int id)
        {
            var question = _context.QuestionsDbSet
                .Include(q => q.Answers)
                .Include(q => q.Tags)
                .FirstOrDefault(q => q.Id == id);
            return question;
        }
        public void PlusQuestion(int id)
        {
            var question = _context.QuestionsDbSet.FirstOrDefault(u => u.Id == id);
            question.Pluses++;
            _context.SaveChanges();
        }
        public void MinusQuestion(int id)
        {
            var question = _context.QuestionsDbSet.FirstOrDefault(u => u.Id == id);
            question.Minuses++;
            _context.SaveChanges();
        }
        public int AddAnswer(AddAnswerDto dto, int id)
        {
            var answer = _mapper.Map<Answer>(dto);
            var question = _context.QuestionsDbSet.FirstOrDefault(q => q.Id == id);
            question.Answers.Add(answer);
            _context.SaveChanges();
            return question.Id;
        }
        public void PlusAnswer(int id)
        {
            var answer = _context.AnswersDbSet.FirstOrDefault(u => u.Id == id);
            answer.Pluses++;
            _context.SaveChanges();
        }
        public void MinusAnswer(int id)
        {
            var answer = _context.AnswersDbSet.FirstOrDefault(u => u.Id == id);
            answer.Minuses++;
            _context.SaveChanges();
        }
    }
}
