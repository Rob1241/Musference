using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musference.Data;
using Musference.Exceptions;
using Musference.Models;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels;

namespace Musference.Services
{
    public interface IQuestionService
    {
        public Task<List<GetQuestionDto>> SearchQuestion(string text);
        public Task<QuestionsResponse> GetAllQuestionsNewest(int page);
        public Task<QuestionsResponse> GetAllQuestionsMostLiked(int page);
        public Task<QuestionsResponse> GetAllQuestionsBestUsers(int page);
        public Task<int> AddQuestion(int userID, AddQuestionDTO questiondto);
        //public IEnumerable<GetQuestionDto> GetAllQuestions();
        public Task<OneQuestionResponse> GetQuestion(int id, int page);

        public void PlusQuestion(int id, int userId);
        //public void MinusQuestion(int id, int userId);
        public Task<int> AddAnswer(AddAnswerDto dto, int id, int userId);
        public void PlusAnswer(int id, int userId);
        //public void ReportQuestion(int id, int userId, string reason);
        //public void ReportAnswer(int id, int userId, string reason);
        //public void MinusAnswer(int id, int userId);
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
        public async Task<List<GetQuestionDto>> SearchQuestion(string text)
        {
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            var listToReturn = await _context.QuestionsDbSet
                                        .Where(c => (c.Content.ToLower().Contains(text.ToLower()))
                                        || c.Heading.ToLower().Contains(text.ToLower()))
                                        .ToListAsync();
            if(listToReturn==null)
            {
                throw new NotFoundException("Questions not found");
            }
            foreach (var item in listToReturn)
            {
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
                questionListDto.Add(getquestiondto);
            }
            return questionListDto;
        }
        public async Task<int> AddQuestion(int userId, AddQuestionDTO questiondto)
        {
            var new_question = _mapper.Map<Question>(questiondto);
            //var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == userId);
            
            new_question.DateAdded = DateTime.Now;
            new_question.User = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);//to chyba wystarczy jesli chodzi o relacje
            if (new_question.User == null)
            {
                throw new NotFoundException("User not found");
            }
            //user.Questions.Add(new_question); a jak nie to jeszcze to
            await _context.QuestionsDbSet.AddAsync(new_question);
            await _context.SaveChangesAsync();
            return new_question.Id;
        }
        //public IEnumerable<GetQuestionDto> GetAllQuestions()
        //{
        //    List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
        //    var questionList = _context.QuestionsDbSet.ToList();
        //    foreach (var item in questionList)
        //    {
        //        var getquestiondto = _mapper.Map<GetQuestionDto>(item);
        //        questionListDto.Add(getquestiondto);
        //    }
        //    return questionListDto;
        //}
        public async Task<QuestionsResponse> GetAllQuestionsNewest(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var sortedquestion = await _context.QuestionsDbSet.OrderBy(q=>q.DateAdded).ToListAsync();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
                questionListDto.Add(getquestiondto);
            }

            var response = new QuestionsResponse
            {
                Questions = questionListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public async Task<QuestionsResponse> GetAllQuestionsMostLiked(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var sortedquestion = await _context.QuestionsDbSet.OrderBy(q => q.Pluses).ToListAsync();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
                questionListDto.Add(getquestiondto);
            }

            var response = new QuestionsResponse
            {
                Questions = questionListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public async Task<QuestionsResponse> GetAllQuestionsBestUsers(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var sortedquestion = await _context.QuestionsDbSet.OrderBy(q => q.User.Reputation).ToListAsync();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetQuestionDto> questionListDto = new List<GetQuestionDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetQuestionDto>(item);
                questionListDto.Add(getquestiondto);
            }

            var response = new QuestionsResponse
            {
                Questions = questionListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public async Task<OneQuestionResponse> GetQuestion(int id, int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var question = await _context.QuestionsDbSet
                .Include(q => q.Answers)
                .Include(q => q.Tags)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new NotFoundException("Question not found");
            }
            var answers = question.Answers
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetAnswerDto> answerListDto = new List<GetAnswerDto>();
            foreach (var item in answers)
            {
                var getanswerdto = _mapper.Map<GetAnswerDto>(item);
                answerListDto.Add(getanswerdto);
            }
            question.Views++;
            var questionDto = _mapper.Map<GetQuestionDto>(question);
            var response = new OneQuestionResponse
            {
                Answers = answerListDto,
                CurrentPage = page,
                Pages = (int)pageCount,
                Question = questionDto
            };
            return response;
        }
        public async void PlusQuestion(int id, int userId)
        {
            var question = await _context.QuestionsDbSet.FirstOrDefaultAsync(u => u.Id == id);
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (question == null)
            {
                throw new NotFoundException("Question not found");
            }
            var usersliked = question.UsersThatLiked;
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                    {
                        //Tu jakis blad
                    }
                }
            }
            user.QuestionsLiked.Add(question);
            question.User.Reputation += 5;
            question.Pluses++;
            await _context.SaveChangesAsync();
        }
        //public void MinusQuestion(int id, int userId)
        //{
        //    var question = _context.QuestionsDbSet.FirstOrDefault(u => u.Id == id);
        //    var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == userId);
        //    var usersliked = question.UsersThatLiked;
        //    if (usersliked != null)
        //    {
        //        foreach (User userloop in usersliked)
        //        {
        //            if (userloop.Id == userId)
        //            {
        //                //Tu jakis blad
        //            }
        //        }
        //    }
        //    user.QuestionsLiked.Add(question);
        //    question.Minuses++;
        //    _context.SaveChanges();
        //}
        public async Task<int> AddAnswer(AddAnswerDto dto, int id, int userId)
        {
            var answer = _mapper.Map<Answer>(dto);
            var question = await _context.QuestionsDbSet.FirstOrDefaultAsync(q => q.Id == id);
            answer.User = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id ==  userId);
            if (question == null)
            {
                throw new NotFoundException("Question not found");
            }
            if (answer.User == null)
            {
                throw new NotFoundException("User not found");
            }
            question.AnswersAmount++;
            answer.Question = question;
            //question.Answers.Add(answer);
            await _context.SaveChangesAsync();
            return question.Id;
        }
        public async void PlusAnswer(int id, int userId)
        {
            var answer = await _context.AnswersDbSet.FirstOrDefaultAsync(u => u.Id == id);
            var user = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                throw new NotFoundException("User not found");
            }
            if (answer == null)
            {
                throw new NotFoundException("Answer not found");
            }
            var usersliked = answer.UsersThatLiked;
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                    {
                        //Tu jakis blad
                    }
                }
            }
            user.AnswersLiked.Add(answer);
            answer.User.Reputation += 5;
            answer.Pluses++;
            await _context.SaveChangesAsync();
        }
        //public void ReportQuestion(int id, int userId, string reason)
        //{

        //}
        //public void ReportAnswer(int id, int userId, string reason)
        //{

        //}
        //public void MinusAnswer(int id, int userId)
        //{
        //    var answer = _context.AnswersDbSet.FirstOrDefault(u => u.Id == id);
        //    var usersliked = answer.UsersThatLiked;
        //    if (usersliked != null)
        //    {
        //        foreach (User user in usersliked)
        //        {
        //            if (user.Id == userId)
        //            {
        //                //Tu jakis blad
        //            }
        //        }
        //    }
        //    answer.Minuses++;
        //    _context.SaveChanges();
        //}
    }
}
