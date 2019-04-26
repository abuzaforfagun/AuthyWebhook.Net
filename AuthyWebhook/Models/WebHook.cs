using System.Collections;
using System.Diagnostics;
using System.Net.Http;

namespace AuthyWebhook.Models
{
    public class WebHook
    {

        public string Name { get; set; }
        public string EventName { get; set; }
        public string CallBackUrl { get; set; }
        public HttpMethod RequestType { get; }

        public string Id { get; set; }
        public WebHook(string name, string eventName, string callBackUrl)
        {
            Name = name;
            EventName = eventName;
            CallBackUrl = callBackUrl;
            RequestType = HttpMethod.Post;

        }

        public WebHook(string Id)
        {
            this.Id = Id;
            RequestType = HttpMethod.Delete;
        }

        public WebHook()
        {
            RequestType = HttpMethod.Get;
        }
    }
}
