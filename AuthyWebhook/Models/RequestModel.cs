using System;
using System.Collections.Generic;
using System.Text;

namespace AuthyWebhook.Models
{
    public class RequestModel
    {
        public string CallbackUrl { get; }
        public string Name { get; }
        public string EventName { get; }
        public string HmacSignature { get; }

        public RequestModel(WebHookConfiguration configuration, string hmacSignature)
        {
            Name = configuration.Name;
            EventName = configuration.EventName;
            HmacSignature = hmacSignature;
            CallbackUrl = configuration.CallBackUrl;
        }

        public RequestModel()
        {
            
        }
    }
}
