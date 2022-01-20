using Silvercrest.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Data.SqlClient;
using System.Net;
using System.IO.Compression;
using System.Web.Configuration;
using Silvercrest.Interfaces.Client;
using Silvercrest.DataAccess.Model;
using Silvercrest.Services.Client;

namespace Silvercrest.Services.CommonServices
{
    public class ScheduleService
    {
        public static void ProceedZips()
        {
            IDocuments _documentsService = new DocumentsService(new SLVR_DEVEntities());
            _documentsService.ProceedZips();
        }
    }
}
