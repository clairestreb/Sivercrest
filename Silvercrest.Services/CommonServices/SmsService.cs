using Microsoft.AspNet.Identity;
using Silvercrest.Interfaces.CommonServices;
using Silvercrest.Services.CommonProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Silvercrest.Services.CommonServices
{
    public class SmsService : IIdentityMessageService
    {
        public async Task SendAsync(IdentityMessage message)
        {
            var twilio = new TwilioProvider();
            var result = await twilio.SendMessage(message.Body, message.Destination);
        }
    }
}
