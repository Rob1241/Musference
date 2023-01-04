using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Musference.Data;
using Musference.Exceptions;
using Musference.Models;
using Musference.Models.DTOs;
using Musference.Models.EndpointModels.Tag;
using System.Security.Cryptography.X509Certificates;

namespace Musference.Services
{
    public interface ITagsService
    {
        //public int AddTag(AddQuestionDTO questiondto);

        public TagResponse GetAllTagsMostPopularTracks(int page);
        public TagResponse GetAllTagsMostPopularQuestion(int page);
        public TagResponse GetAllTagsName(int page);
        public IEnumerable<GetTagDto> SearchTag(string text);
    }
    public class TagsService : ITagsService
    {
        
        private readonly DataBaseContext _context;
        public readonly IMapper _mapper;

        public TagsService(DataBaseContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }
        public TagResponse GetAllTagsMostPopularTracks(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TagsDbSet.Count() / pageResults);
            var sortedquestion = _context.TagsDbSet.OrderBy(q => q.Tracks.Count()).ToList();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTagDto> tagListDto = new List<GetTagDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetTagDto>(item);
                tagListDto.Add(getquestiondto);
            }
            var response = new TagResponse
            {
                Tags = tagListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public TagResponse GetAllTagsMostPopularQuestion(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TagsDbSet.Count() / pageResults);
            var sortedquestion = _context.TagsDbSet.OrderBy(q => q.Questions.Count()).ToList();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTagDto> tagListDto = new List<GetTagDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetTagDto>(item);
                tagListDto.Add(getquestiondto);
            }
            var response = new TagResponse
            {
                Tags = tagListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public TagResponse GetAllTagsName(int page)
        {
            var pageResults = 10f;
            var pageCount = Math.Ceiling(_context.TagsDbSet.Count() / pageResults);
            var sortedquestion = _context.TagsDbSet.OrderBy(q => q.Description).ToList();
            var questions = sortedquestion
                .Skip((page - 1) * (int)pageResults)
                .Take((int)pageResults)
                .ToList();
            List<GetTagDto> tagListDto = new List<GetTagDto>();
            foreach (var item in questions)
            {
                var getquestiondto = _mapper.Map<GetTagDto>(item);
                tagListDto.Add(getquestiondto);
            }
            var response = new TagResponse
            {
                Tags = tagListDto,
                CurrentPage = page,
                Pages = (int)pageCount
            };

            return response;
        }
        public IEnumerable<GetTagDto> SearchTag(string text)
        {
            List<GetTagDto> questionListDto = new List<GetTagDto>();
            var listToReturn = _context.TagsDbSet
                                        .Where(t => (t.Description.ToLower().Contains(text.ToLower())))
                                        .ToList();
            if (listToReturn == null)
            {
                throw new NotFoundException("Questions not found");
            }
            foreach (var item in listToReturn)
            {
                var getquestiondto = _mapper.Map<GetTagDto>(item);
                questionListDto.Add(getquestiondto);
            }
            return questionListDto;
        }

    }
}
