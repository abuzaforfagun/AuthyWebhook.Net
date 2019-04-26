using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AuthyWebhook.Models
{
    public class RequestModel
    {
        public RequestConfiguration RequestConfiguration { get; }
        public string CallbackUrl { get; }
        public string Name { get; }
        public string EventName { get; }

        public RequestModel(WebHookConfiguration configuration, RequestConfiguration requestConfiguration)
        {
            RequestConfiguration = requestConfiguration;
            Name = configuration.Name;
            EventName = configuration.EventName;
            CallbackUrl = configuration.CallBackUrl;
        }

        public RequestModel(RequestConfiguration requestConfiguration)
        {
            RequestConfiguration = requestConfiguration;
        }
    }
}
