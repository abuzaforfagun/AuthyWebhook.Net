using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator : IAuthyWebHookGenerator
    {
        private ICryptographyHelper cryptographyHelper;
        private readonly  string nonce;
        private readonly AuthyConfiguration configuration;

        public AuthyWebHookGenerator(AuthyConfiguration configuration)
        {
            this.configuration = configuration;

            nonce = Guid.NewGuid().ToString();

            cryptographyHelper = new CryptographyHelper();
        }

        public async Task<string> CreateWebhooksAsync(WebHookConfiguration webHookConfiguration)
        {

            string method = "POST";
            var sortedParams =
                $"access_key={configuration.AccessKey}&app_api_key={configuration.ApiKey}&events%5B%5D={webHookConfiguration.EventName}&name={webHookConfiguration.Name}&url={Uri.EscapeDataString(webHookConfiguration.CallBackUrl)}";

            string dataToSign = $"{nonce}|{method}|{Constants.AUTHY_WEBHOOK_URL}|{sortedParams}";

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            
            var requestModel = new RequestModel(webHookConfiguration, hmacSignature, nonce);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest(requestModel);

        }

        public string CreateWebhooks(WebHookConfiguration webHookConfiguration)
        {
            return CreateWebhooksAsync(webHookConfiguration).GetAwaiter().GetResult();
        }
    }
}
