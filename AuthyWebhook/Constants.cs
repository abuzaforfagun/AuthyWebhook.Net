namespace AuthyWebhook
{
    public static class Constants
    {
        public const string AUTHY_WEBHOOK_URL = "https://api.authy.com/dashboard/json/application/webhooks";

        public static string GetAuthyUrl(string webHookId = null)
        {
            if (webHookId != null)
            {
                return $"{AUTHY_WEBHOOK_URL}/{webHookId}";
            }

            return AUTHY_WEBHOOK_URL;
        }

    }
}
