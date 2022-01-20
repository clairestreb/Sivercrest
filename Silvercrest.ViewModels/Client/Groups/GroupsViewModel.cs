using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.ViewModels.Client.Groups
{
    public class GroupsViewModel:GenericViewModel
    {
        public List<int> GroupIds { get; set; }
        public List<AccountGroupMemberViewModel> Accounts { get; set; } //Accounts and what groups they belong to
        public Dictionary<int, string> GroupAccounts { get; set; }  //All Accounts

        public Dictionary<int, string> Groups { get; set; } //All groups

        public int ContactId;
        public string FullName;

        public GroupsViewModel()
        {
            Accounts = new List<AccountGroupMemberViewModel>();
            GroupAccounts = new Dictionary<int, string>();
            GroupIds = new List<int>();
            Groups = new Dictionary<int, string>();
        }

        public string getAllGroups()
        {
            string str = string.Empty;
            int groupId;

            if (GroupIds.Count > 0)
            {
                for (int i = 0; i < GroupIds.Count; i++)
                {
                    groupId = GroupIds[i];
                    str = str + groupId.ToString() + ":" + Groups[groupId] + ";";
                }
            }
            else
            {
                str = ";";
            }
                return str.Substring(0, str.Length - 1);
        }
    }

}
