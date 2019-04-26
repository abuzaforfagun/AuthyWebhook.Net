namespace AuthyWebhook.Models
{
    public class AuthyConfiguration
    {
        public string ApiKey { get; set; }
        public string AccessKey { get; set; }
        public string SigninKey { get; set; }

        public AuthyConfiguration(string apiKey, string accessKey, string signinKey)
        {
            ApiKey = apiKey;
            AccessKey = accessKey;
            SigninKey = signinKey;
        }
    }
}
