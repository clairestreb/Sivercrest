using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Interfaces.CommonProviders
{
    public interface ITwilioProvider
    {
        Task<bool> SendMessage(string messageText, string receiverPhoneNumber);
    }
}
