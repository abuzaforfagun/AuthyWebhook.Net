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

        public async Task<T> CreateWebhooksAsync<T>(WebHook webHook)
        {
            string dataToSign = GetDataToSign(webHook);

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(webHook, requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        public async Task<T> GetAuthyWebhooksAsync<T>()
        {
            var webHook = new WebHook();

            string dataToSign = GetDataToSign(webHook);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        public async Task<bool> DeleteWebHookAsync(string webHookId)
        {
            var webHook = new WebHook(webHookId);

            string dataToSign = GetDataToSign(webHook);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(webHook, requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return (await authyClient.SendHttpRequest<Response>(requestModel)).success;
        }

        private string GetDataToSign(WebHook webHook)
        {
            var sortedParams = $"access_key={configuration.AccessKey}&app_api_key={configuration.ApiKey}";
            if (webHook != null && webHook.CallBackUrl != null)
            {
                sortedParams += $"&events%5B%5D={webHook.EventName}&name={webHook.Name}&url={Uri.EscapeDataString(webHook.CallBackUrl)}";
            }

            return $"{nonce}|{webHook.RequestType}|{Constants.GetAuthyUrl(webHook.Id)}|{sortedParams}"; ;
        }

        public T CreateWebhooks<T>(WebHook webHook)
        {
            return CreateWebhooksAsync<T>(webHook).GetAwaiter().GetResult();
        }

        public async Task<string> CreateWebhooksAsync(WebHook webHook)
        {
            return await CreateWebhooksAsync<string>(webHook);
        }

        public string CreateWebhooks(WebHook webHook)
        {
            return CreateWebhooks<string>(webHook);
        }

        public T GetAuthyWebhooks<T>()
        {
            return GetAuthyWebhooksAsync<T>().GetAwaiter().GetResult();
        }

        public string GetAuthyWebhooks()
        {
            return GetAuthyWebhooksAsync<string>().GetAwaiter().GetResult();
        }

        public bool DeleteWebHook(string webHookId)
        {
            return DeleteWebHookAsync(webHookId).GetAwaiter().GetResult();
        }
    }
}
