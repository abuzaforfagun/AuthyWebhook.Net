using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookHelper : IAuthyWebHookHelper
    {
        private ICryptographyHelper cryptographyHelper;
        private readonly string nonce;
        private readonly AuthyConfiguration configuration;
        public List<Response> WebHooks { get; set; }

        public AuthyWebHookHelper(AuthyConfiguration configuration)
        {
            this.configuration = configuration;

            nonce = Guid.NewGuid().ToString();

            cryptographyHelper = new CryptographyHelper();
        }

        public async Task<T> CreateWebhooksAsync<T>(WebHookConfiguration webHookConfiguration)
        {
            var _requestType = "POST";
            
            string dataToSign = GetDataToSign(_requestType, webHookConfiguration);

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce, HttpMethod.Post);
            var requestModel = new RequestModel(webHookConfiguration, requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        public async Task<T> GetAuthyWebhooksAsync<T>()
        {
            var _requestType = "GET";

            string dataToSign = GetDataToSign(_requestType);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        private string GetDataToSign(string requestType, WebHookConfiguration webHookConfiguration = null)
        {
            var sortedParams = $"access_key={configuration.AccessKey}&app_api_key={configuration.ApiKey}";
            if (webHookConfiguration != null && webHookConfiguration.CallBackUrl != null)
            {
                sortedParams +=
                    $"&events%5B%5D={webHookConfiguration.EventName}&name={webHookConfiguration.Name}&url={Uri.EscapeDataString(webHookConfiguration.CallBackUrl)}";
            }

            return $"{nonce}|{requestType}|{Constants.AUTHY_WEBHOOK_URL}|{sortedParams}"; ;
        }

        public T CreateWebhooks<T>(WebHookConfiguration webHookConfiguration)
        {
            return CreateWebhooksAsync<T>(webHookConfiguration).GetAwaiter().GetResult();
        }

        public async Task<string> CreateWebhooksAsync(WebHookConfiguration webHookConfiguration)
        {
            return await CreateWebhooksAsync<string>(webHookConfiguration);
        }

        public string CreateWebhooks(WebHookConfiguration webHookConfiguration)
        {
            return CreateWebhooks<string>(webHookConfiguration);
        }

        public T GetAuthyWebhooks<T>()
        {
            return GetAuthyWebhooksAsync<T>().GetAwaiter().GetResult();
        }

        public string GetAuthyWebhooks()
        {
            return GetAuthyWebhooksAsync<string>().GetAwaiter().GetResult();
        }
    }
}
