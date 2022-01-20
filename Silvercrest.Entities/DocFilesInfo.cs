using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Entities
{
    public class DocFilesInfo
    {
        public string SourceFoler { get; set; }
        public string DestFolder { get; set; }

        public string ArchiveFolder { get; set; }
        public List<string> FileNames { get; set; }

    }
}
