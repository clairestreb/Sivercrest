using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Silvercrest.DataAccess.Repositories;
using Silvercrest.Interfaces;
using Silvercrest.ViewModels.Client;
using Silvercrest.Entities;
using System.Diagnostics;
using Silvercrest.Utilities;
using System.Web.Mvc;
using Silvercrest.ViewModels.Client.Holdings.Index;
using Silvercrest.ViewModels.Client.Holdings;
using System.Data;
using Silvercrest.DataAccess.Mappers;

namespace Silvercrest.Services.Client
{
    public class ClientAdoService: IClientAdoService
    {
        private ClientRepository _repository;

        public ClientAdoService()
        {
            _repository = new ClientRepository();
        }

        public void ContactEntities(List<ClientAccount> entities, int contactId)
        {
            DataSet data = _repository.ContactEntities(contactId);
            List<ClientAccount> list = new List<ClientAccount>();
            ClientAdoMapper.MapAccounts(data.Tables[0].Rows, list);
            entities.AddRange(list.Where(e =>(bool) e.IsGroup)) ;
        }

        public void FillHomeView(IndexInitViewModel model, List<PieChart> charts, UserInfo info, List<ClientGroupAccount> mappedAccountList)
        {
            DataSet data = _repository.FillHomeView(info.ContactId, info.EntityId, info.IsGroup, info.IsClientGroup, info.SubEntityId);
            ClientAdoMapper.MapGroupAccounts(data.Tables[0].Rows, mappedAccountList);
            ClientAdoMapper.MapCharts(data.Tables[1].Rows, charts);
            ClientAdoMapper.MapViewInfo(data.Tables[1].Rows, model);
        }
    }
}
