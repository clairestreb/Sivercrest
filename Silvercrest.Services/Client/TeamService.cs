using Silvercrest.Interfaces;
using System.Collections.Generic;
using Silvercrest.ViewModels.Client;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using System.Linq;
using System;

namespace Silvercrest.Services
{
    public class TeamService : ITeam
    {
        private GroupRepository _groupRepository;
        private AccountRepository _accountRepository;
        public TeamService(SLVR_DEVEntities context)
        {
            _groupRepository = new GroupRepository(context);
            _accountRepository = new AccountRepository(context);
        }

        public List<ClientTeamViewModel> GetClientTeam(int? contactId)
        {
            var clientTeam = _groupRepository.GetClientTeam(contactId).ToList();
            var clientTeamList = new List<ClientTeamViewModel>();
            foreach (var team in clientTeam)
            {
                var clientTeamViewModel = new ClientTeamViewModel();
                clientTeamViewModel.Name = team.Name;
                clientTeamViewModel.Title = team.Title;
                clientTeamViewModel.Email = team.Email;
                clientTeamViewModel.PhoneNumber = team.PhoneNumber;
                clientTeamViewModel.Photo = team.Photo;
                clientTeamList.Add(clientTeamViewModel);
            };
            return clientTeamList;
        }

        public int? GetContactId(string name)
        {
            return _accountRepository.GetContactId(name);
        }
    }
}
