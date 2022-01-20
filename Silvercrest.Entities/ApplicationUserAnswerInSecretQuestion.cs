using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class ApplicationUserAnswerInSecretQuestion
    {
        public int Id { get; set; }
        public virtual SecretQuestion SecretQuestion { get; set; }
        public string SecretQuestionAnswerHash { get; set; }
    }
}
