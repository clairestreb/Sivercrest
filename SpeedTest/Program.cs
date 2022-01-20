using Silvercrest.DataAccess.Model;
using Silvercrest.Entities;
using Silvercrest.Services;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SpeedTest
{
    class Program
    {
        static void Main(string[] args)
        {
            
           

            
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            var context = new SLVR_DEVEntities1();
            var service = new ClientService(context);
            //string connectionString = System.Configuration.ConfigurationManager.ConnectionStrings["Database"].ConnectionString;
            //using (SqlConnection connection = new SqlConnection("Server=v3iinihz9y.database.windows.net;Database=SLVR_DEV;User ID=Silvercrest;Password=Welcome2Dev;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;"))
            //{
            //    using (SqlCommand cmd = new SqlCommand("p_Web_Contact_Entities", connection))
            //    {
            //        cmd.CommandType = CommandType.StoredProcedure;
            //        cmd.Parameters.Add("contact_id", SqlDbType.Int).Value = 6414;
            //        connection.Open();
            //        cmd.ExecuteNonQuery();
            //    }
            //}
            var model = new ClientAccount();
            service.GetAccounts(6414);
            stopWatch.Stop();
            var time = stopWatch.Elapsed.TotalSeconds;
        }
    }
}
