namespace AuthyWebhook
{
    public interface ICryptographyHelper
    {
        string GenerateHmac(string message, string key);
    }
}