using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Repositories
{
    public class SecretQuestionRepository
    {
        private SLVR_DEVEntities1 _context;

        public SecretQuestionRepository(SLVR_DEVEntities1 context)
        {
            _context = context;
        }

        public List<Silvercrest.Entities.SecretQuestion> GetQuestionsList()
        {
            using (var ctx = _context)
            {
                IList<Web_Security_Question> questionList = ctx.Web_Security_Question.ToList();
                var mappedList = SecretQuestionMapper.MapQuestionsList(questionList);
                return mappedList;
            }
        }
    }
}
