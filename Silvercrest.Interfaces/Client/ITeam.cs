using Silvercrest.ViewModels.Client;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces
{
    public interface ITeam
    {
        List<ClientTeamViewModel> GetClientTeam(int? contactId);
        int? GetContactId(string name);
    }
}
