using System;
using System.Collections.Generic;
using System.Text;

namespace AuthyWebhook.Models
{
    public class WebHookConfiguration
    {

        public string Name { get; set; }
        public string EventName { get; set; }
        public string CallBackUrl { get; set; }
        public WebHookConfiguration(string name, string eventName, string callBackUrl)
        {
            Name = name;
            EventName = eventName;
            CallBackUrl = callBackUrl;
        }

        public WebHookConfiguration()
        {
            
        }

    }
}
