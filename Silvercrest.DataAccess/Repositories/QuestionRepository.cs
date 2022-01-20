using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class QuestionRepository
    {
        private SLVR_DEVEntities _context;

        public QuestionRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public List<SecretQuestion> GetQuestionsList()
        {
            var listQuestions = _context.Web_Security_Question.ToList();
            var mappedList = QuestionMapper.MapQuestionList(listQuestions);
            return mappedList;
        }
    }
}
