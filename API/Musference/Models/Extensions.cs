using Musference.Data;
using Musference.Models.DTOs;

namespace Musference.Models
{
    public class Extensions
    {

        public readonly DataBaseContext _context;

        public Extensions(DataBaseContext context)
        {
            _context = context;
        }
        public GetQuestionDto AsDto(Question question)
        {
            var user = _context.UsersDbSet.FirstOrDefault(u => u.Id == question.UserId);
            return new GetQuestionDto()
            {
                Heading = question.Heading,
                Content = question.Content,
                DateAdded = question.DateAdded,
                Pluses = question.Pluses,
                Minuses = question.Minuses,

                Username = user.Name,

                Tags = question.Tags,

                Answers = question.Answers,

                Views = question.Views
            };
        }
    }
}
