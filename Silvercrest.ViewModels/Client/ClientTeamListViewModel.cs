using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client
{
    public class ClientTeamListViewModel:GenericViewModel
    {
        public List<ClientTeamViewModel> teamModels { get; set; }
    }
}
