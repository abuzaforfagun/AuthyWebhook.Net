namespace AuthyWebhook.Models
{
    public class CryptoConfiguration
    {
        public string HmacSignature { get; }
        public string Nonce { get; }
        public CryptoConfiguration(string hmacSignature, string nonce)
        {
            HmacSignature = hmacSignature;
            Nonce = nonce;
        }
    }
}
