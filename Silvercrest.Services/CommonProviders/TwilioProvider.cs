using Silvercrest.Interfaces.CommonProviders;
using Silvercrest.Utilities;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;
using Twilio;
using Twilio.Base;
using Twilio.Rest.Api.V2010.Account;

namespace Silvercrest.Services.CommonProviders
{
    public class TwilioProvider : ITwilioProvider
    {
        private readonly string _messagingServiceId = !String.IsNullOrEmpty(WebConfigurationManager.AppSettings["messagingServiceId"]) ? WebConfigurationManager.AppSettings["messagingServiceId"] : null;
        private readonly string _fromPhoneNumber = WebConfigurationManager.AppSettings["fromPhoneNumber"];
        private readonly string _accountSid = WebConfigurationManager.AppSettings["accountSid"];
        private readonly string _authToken = WebConfigurationManager.AppSettings["authToken"];

        public TwilioProvider()
        {
            TwilioClient.Init(_accountSid, _authToken);
        }

        public async Task<bool> SendMessage(string messageText, string receiverPhoneNumber)
        {
            try
            {
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;
                var sendingResult = await MessageResource.CreateAsync(
                    body: messageText,
                    messagingServiceSid: _messagingServiceId,
                    from: new Twilio.Types.PhoneNumber(_fromPhoneNumber),
                    to: new Twilio.Types.PhoneNumber(receiverPhoneNumber)
                );

                if (sendingResult.ErrorCode != null)
                {
                    throw new Exception($"Twilio: {sendingResult.ErrorCode.Value} {sendingResult.ErrorMessage}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                return false;
            }
            return true;
        }
    }
}
