using Silvercrest.DataAccess.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.DataAccess.Mappers
{
    public static class QuestionMapper
    {
        public static List<Silvercrest.Entities.SecretQuestion> MapQuestionList(IList<Web_Security_Question> list)
        {
            var mappedQuestionList = new List<Silvercrest.Entities.SecretQuestion>();
            foreach (var question in list)
            {
                var mappedQuestion = new Silvercrest.Entities.SecretQuestion();
                mappedQuestion.Id = question.id;
                mappedQuestion.Question = question.question;
                mappedQuestionList.Add(mappedQuestion);
            }
            return mappedQuestionList;
        }
    }
}
