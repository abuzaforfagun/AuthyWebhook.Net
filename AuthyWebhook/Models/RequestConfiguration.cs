namespace AuthyWebhook.Models
{
    public class RequestConfiguration
    {
        public string HmacSignature { get; }
        public string Nonce { get; }
        public RequestConfiguration(string hmacSignature, string nonce)
        {
            HmacSignature = hmacSignature;
            Nonce = nonce;
        }
    }
}
