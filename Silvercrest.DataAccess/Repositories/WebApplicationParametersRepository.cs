using Silvercrest.DataAccess.Model;
using System.Linq;

namespace Silvercrest.DataAccess.Repositories
{
    public class WebApplicationParametersRepository
    {
        private SLVR_DEVEntities _context;
        public WebApplicationParametersRepository(SLVR_DEVEntities context)
        {
            _context = context;
        }

        public Web_Application_Parameters GetApplicationParameters()
        {
            var result = _context.Web_Application_Parameters.FirstOrDefault();
            return result;
        }
    }
}
