namespace AuthyWebhook.Models
{
    public class RequestModel
    {
        public RequestConfiguration RequestConfiguration { get; }
        public WebHook WebHook { get; set; }

        public RequestModel(WebHook webHook, RequestConfiguration requestConfiguration)
        {
            RequestConfiguration = requestConfiguration;
            WebHook = webHook;
        }

        public RequestModel(RequestConfiguration requestConfiguration)
        {
            RequestConfiguration = requestConfiguration;
            WebHook = new WebHook();
        }
    }
}
