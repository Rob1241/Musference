using AutoMapper;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels.Question;
using Musference.Models.EndpointModels.Track;
using Musference.Models.EndpointModels.User;
using Musference.Models.Entities;

namespace Musference.Logic
{
    public interface IPagination
    {
        public UsersResponse UserPagination(List<User> sortedusers, float pageResults, int page, double pageCount);
        public TrackResponse TrackPagination(List<Track> sortedtrack, float pageResults, int page, double pageCount);
        public OneQuestionResponse AnswerPagination(List<Answer> answers, float pageResults, int page, double pageCount, Question question);
        public QuestionsResponse QuestionPagination(List<Question> sortedquestion, float pageResults, int page, double pageCount);
    }

    public class Pagination : IPagination
    {
        public readonly IMapper _mapper;
        public Pagination(IMapper mapper)
        {
              _mapper = mapper;
        }
        public UsersResponse UserPagination(List<User> sortedusers, float pageResults, int page, double pageCount) 
        {
            var users = sortedusers
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            var usersdto = new List<GetAllUsersDto>();
            foreach (var item in users)
            {
                usersdto.Add(_mapper.Map<GetAllUsersDto>(item));
            }
            if(page>(int)pageCount)
            {
                throw new Exception("Page not found");
            }
            var response = new UsersResponse
            {
                Users = usersdto,
                CurrentPage = page,
                Pages = (int)pageCount
            };
            return response;
        }
        public TrackResponse TrackPagination(List<Track> sortedtrack, float pageResults, int page, double pageCount)
        {
            var tracks = sortedtrack
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTrackDto> trackListDto = new List<GetTrackDto>();
            if (page > (int)pageCount)
            {
                throw new Exception("Page not found");
            }
            foreach (var item in tracks)
            {
                var gettrackdto = _mapper.Map<GetTrackDto>(item);
                trackListDto.Add(gettrackdto);
            }

            var response = new TrackResponse
            {
                Tracks = trackListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public OneQuestionResponse AnswerPagination(List<Answer> answers, float pageResults, int page, double pageCount, Question question)
        {
            List<GetAnswerDto> answerListDto = new List<GetAnswerDto>();
            if (page > (int)pageCount)
            {
                throw new Exception("Page not found");
            }
            foreach (var item in answers)
            {
                var getanswerdto = _mapper.Map<GetAnswerDto>(item);
                answerListDto.Add(getanswerdto);
            }
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
        public QuestionsResponse QuestionPagination(List<Question> sortedquestion, float pageResults,int page, double pageCount) 
        {
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetAllQuestionDto> questionListDto = new List<GetAllQuestionDto>();
            if (page > (int)pageCount)
            {
                throw new Exception("Page not found");
            }
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetAllQuestionDto>(item);
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

    }
}
