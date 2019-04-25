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
        public string Nonce { get; }

        public RequestModel(WebHookConfiguration configuration, string hmacSignature, string nonce)
        {
            Name = configuration.Name;
            EventName = configuration.EventName;
            HmacSignature = hmacSignature;
            Nonce = nonce;
            CallbackUrl = configuration.CallBackUrl;
        }

        public RequestModel()
        {
            
        }
    }
}
