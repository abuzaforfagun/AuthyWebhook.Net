namespace AuthyWebhook.Models
{
    public class Request
    {
        public CryptoConfiguration CryptoConfiguration { get; }
        public WebHook WebHook { get; set; }

        public Request(WebHook webHook, CryptoConfiguration cryptoConfiguration)
        {
            CryptoConfiguration = cryptoConfiguration;
            WebHook = webHook;
        }

        public Request(CryptoConfiguration cryptoConfiguration)
        {
            CryptoConfiguration = cryptoConfiguration;
            WebHook = new WebHook();
        }
    }
}
