using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Musference.Data;
using Musference.Exceptions;
using Musference.Logic;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels.Question;
using Musference.Models.Entities;

namespace Musference.Services
{
    public interface IQuestionService
    {
        public Task<QuestionsResponse> SearchQuestion(string text,int page);
        public Task<QuestionsResponse> GetAllQuestionsNewest(int page);
        public Task<QuestionsResponse> GetAllQuestionsMostLiked(int page);

        public Task<int> AddQuestion(int userID, AddQuestionDTO questiondto);
        public Task<OneQuestionResponse> GetQuestion(int id, int page);

        public void PlusQuestion(int id, int userId);
        public void DeleteQuestion(int id, int userId);
        public void DeleteAnswer(int id, int userId);
        public Task<int> AddAnswer(AddAnswerDto dto, int id, int userId);
        public void PlusAnswer(int id, int userId);
    }
    public class QuestionService : IQuestionService
    {
        public readonly DataBaseContext _context;
        public readonly IMapper _mapper;
        public readonly IPagination _pagination;
        public QuestionService(DataBaseContext context, IMapper mapper, IPagination pagination)
        {
            _context = context;
            _mapper = mapper;
            _pagination = pagination;
        }
        public async Task<QuestionsResponse> SearchQuestion(string text, int page)
        {
            var pageResults = 25f;
            var questionList = await _context.QuestionsDbSet
                                        .Where(c => (c.Content.ToLower().Contains(text.ToLower()))
                                        || c.Heading.ToLower().Contains(text.ToLower()))
                                        .ToListAsync();
            var pageCount = Math.Ceiling(questionList.Count() / pageResults);
            if (questionList==null)
            {
                throw new NotFoundException("Questions not found");
            }
            var response = _pagination.QuestionPagination(questionList, pageResults, page, pageCount);
            return response;
        }
        public async Task<int> AddQuestion(int userId, AddQuestionDTO questiondto)
        {
            var new_question = _mapper.Map<Question>(questiondto);
            new_question.DateAdded = DateTime.Now;
            new_question.User = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == userId);
            if (new_question.User == null)
                throw new NotFoundException("User not found");
            await _context.QuestionsDbSet.AddAsync(new_question);
            await _context.SaveChangesAsync();
            return new_question.Id;
        }
        public async Task<QuestionsResponse> GetAllQuestionsNewest(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var sortedquestion = await _context.QuestionsDbSet
                .Include(q=>q.User)
                .OrderByDescending(q=>q.DateAdded).ToListAsync();
            var response = _pagination.QuestionPagination(sortedquestion, pageResults, page, pageCount);
            return response;
        }
        public async Task<QuestionsResponse> GetAllQuestionsMostLiked(int page)
        {
            var pageResults = 25f;
            var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            var sortedquestion = await _context.QuestionsDbSet
                .Include(q=>q.User)
                .OrderByDescending(q => q.Pluses)
                .Take(20)
                .ToListAsync();
            var response = _pagination.QuestionPagination(sortedquestion, pageResults, page, pageCount);
            return response;
        }
        public async Task<OneQuestionResponse> GetQuestion(int id, int page)
        {
            var pageResults = 7f;
            //var pageCount = Math.Ceiling(_context.QuestionsDbSet.Count() / pageResults);
            
            var question = await _context.QuestionsDbSet
                .Include(q=>q.User)
                .Include(q => q.Answers)
                .ThenInclude(a=>a.User)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
            {
                throw new NotFoundException("Question not found");
            }
            question.Views++;
            await _context.SaveChangesAsync();
            var answers = question.Answers
                .OrderByDescending(a=>a.DateAdded)
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            var pageCount = Math.Ceiling(question.Answers.Count() / pageResults);
            if(pageCount == 0)
            {
                pageCount = 1;
            }
            var answersAmount = question.Answers.Count();
            var response = _pagination.AnswerPagination(answers, pageResults, page, pageCount,question,answersAmount);
            return response;
        }
        public async void PlusQuestion(int id, int userId)
        {
            var question = await _context.QuestionsDbSet
                .Include(q=>q.UsersThatLiked)
                .Include(q=>q.User)
                .FirstOrDefaultAsync(q => q.Id == id);
            var user = await _context.UsersDbSet
                .Include(u=>u.QuestionsLiked)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)throw new NotFoundException("User not found");
            if (question == null)throw new NotFoundException("Question not found");
            var usersliked = question.UsersThatLiked;
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                    {
                        throw new NotFoundException("You can like only once");
                    }
                }
            }
            if(user.QuestionsLiked==null)throw new NotFoundException("empty questions");
            user.QuestionsLiked.Add(question);
            question.User.Reputation += 5;
            question.Pluses++;
            await _context.SaveChangesAsync();
        }
        public async Task<int> AddAnswer(AddAnswerDto dto, int id, int userId)
        {
            var new_answer = _mapper.Map<Answer>(dto);
            new_answer.Question = await _context.QuestionsDbSet
                .FirstOrDefaultAsync(q => q.Id == id);
            new_answer.User = await _context.UsersDbSet
                .FirstOrDefaultAsync(u => u.Id == userId);
            new_answer.DateAdded = DateTime.Now;
            if (new_answer.Question == null)
                throw new NotFoundException("Question not found");
            if (new_answer.User == null)
                throw new NotFoundException("User not found");
            new_answer.Question.AnswersAmount++;
            await _context.AnswersDbSet.AddAsync(new_answer);
            await _context.SaveChangesAsync();
            return new_answer.Id;
        }
        public async void PlusAnswer(int id, int userId)
        {
            var answer = await _context.AnswersDbSet
                .Include(a=>a.UsersThatLiked)
                .FirstOrDefaultAsync(a => a.Id == id);
            var user = await _context.UsersDbSet
                .Include(u=>u.AnswersLiked)
                .FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
                throw new NotFoundException("User not found");
            if (answer == null)
                throw new NotFoundException("Answer not found");
            var usersliked = answer.UsersThatLiked;
            if (usersliked != null)
            {
                foreach (User userloop in usersliked)
                {
                    if (userloop.Id == userId)
                        throw new NotFoundException("You can like only once");
                }
            }
            user.AnswersLiked.Add(answer);
            var answeruser = await _context.UsersDbSet.FirstOrDefaultAsync(u => u.Id == answer.UserId);
            answeruser.Reputation += 5;
            answer.Pluses++;
            await _context.SaveChangesAsync();
        }
        public async void DeleteQuestion(int id, int userId)
        {
            var question = await _context.QuestionsDbSet
                .Include(q=>q.Answers)
                .FirstOrDefaultAsync(q => q.Id == id);
            if (question == null)
                throw new NotFoundException("Question not found");
            if (question.UserId != userId)
                throw new UnauthorizedException("Unauthorizes method");
            var answers = question.Answers;
            foreach(Answer answer in answers)
            {
                _context.AnswersDbSet.Remove(answer);
            }
            _context.QuestionsDbSet.Remove(question);
            await _context.SaveChangesAsync();
        }
        public async void DeleteAnswer(int id, int userId)
        {   
            var answer = await _context.AnswersDbSet
                .Include(a=>a.Question)
                .FirstOrDefaultAsync(a => a.Id == id);
            if (answer == null)
                throw new NotFoundException("Answer not found");
            if(answer.UserId != userId)
                throw new UnauthorizedException("Unauthorizes method");
            answer.Question.AnswersAmount--;
            _context.AnswersDbSet.Remove(answer);
            await _context.SaveChangesAsync();
        }
    }
}
