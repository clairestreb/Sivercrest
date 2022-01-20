using Silvercrest.DataAccess.Mappers;
using Silvercrest.DataAccess.Model;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Entities;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.Client.Groups;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services
{
    public class GroupService : IGroupService
    {
        private GroupRepository _groupRepository;
        private AccountRepository _accountRepository;
        private ClientRepository _clientRepository;

        public GroupService(SLVR_DEVEntities context, IClientAdoService adoService)
        {
            _groupRepository = new GroupRepository(context);
            _accountRepository = new AccountRepository(context);
            _clientRepository = new ClientRepository();
        }

        public GroupsViewModel GetGroupsList(int? contactId)
        {
            DataSet data = _clientRepository.ClientGroups(contactId);
            List<Group> group = new List<Group>();
            GroupAdoMapper.MapGroups(data.Tables[0].Rows, group);
            var model = new GroupsViewModel();
            IEnumerable<Group> sortedEnum = group.OrderBy(f => f.GroupName).OrderBy(g => g.AccountName);
            var grps = sortedEnum.ToList();

            model.GroupIds = grps.Select(x => x.AccountGroupId).Distinct().ToList();

            grps.GroupBy(x => x.AccountGroupId)
                .Select(i => i.First())
                .ToList()
                .ForEach(x => model.Groups.Add(x.AccountGroupId, x.GroupName ));

            model.ContactId = contactId ?? 0;
            model.FullName = _accountRepository.GetUserName(contactId);
            GroupAdoMapper.MapAccountGroups(data.Tables[1].Rows, model.GroupAccounts);
            foreach (var g in grps)
            {
                model.Accounts.Add(new AccountGroupMemberViewModel
                {
                    AccountName = g.AccountName,
                    IsGroupMember = g.IsGroupMember,
                    AccountId = g.AccountId,
                    AccountGroupId = g.AccountGroupId
                });
            }
            return model;
        }

        public void UpdateGroup(int? contactId, string groupName, string accountIds, string changerName, int? accountGroupId)
        {
/*
            if (accountGroupId != null)
            {
                DataSet data = _clientRepository.ClientGroups(contactId);
                List<Group> group = new List<Group>();
                GroupAdoMapper.MapGroups(data.Tables[0].Rows, group);
                accountGroupId = group
               .Where(x => x.GroupName == oldName)
               .Select(x => x.AccountGroupId)
               .First();
            }
*/
            _groupRepository.UpdateClientGroup(contactId, groupName, accountIds, changerName, accountGroupId);
        }

        public void DeleteGroup(int? contactId, int? accountGroupId)
        {
/*
            DataSet data = _clientRepository.ClientGroups(contactId);
            List<Group> group = new List<Group>();
            GroupAdoMapper.MapGroups(data.Tables[0].Rows, group);
            int accountGroupId = group
           .Where(x => x.GroupName == oldName)
           .Select(x => x.AccountGroupId)
           .First();
*/
            _groupRepository.DeleteClientGroup(contactId, accountGroupId);
        }

    }
}
