using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UploadZip
{
    class AdoMapper
    {
        public static void Mapper(List<EmailAlertViewModel> model, DataSet data)
        {
            foreach (DataRow row in data.Tables[0].Rows)
            {
                var mapped = new EmailAlertViewModel();
                mapped.Id = (int)row[0];
                mapped.Recipient = row[1].ToString();
                mapped.FileId = row[2].ToString();
                mapped.DisplayName = row[3].ToString();
                mapped.DocumentDate = (DateTime)row[4];
                mapped.DocumentType = row[5].ToString();
                mapped.ClientName = row[6].ToString();
                model.Add(mapped);
            }
        }

    }
}
