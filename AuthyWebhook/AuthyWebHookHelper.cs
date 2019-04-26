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

        public async Task<T> CreateAsync<T>(WebHook webHook)
        {
            string dataToSign = GetDataToSign(webHook);

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(webHook, requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        public async Task<T> GetAsync<T>()
        {
            var webHook = new WebHook();

            string dataToSign = GetDataToSign(webHook);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var requestConfiguration = new RequestConfiguration(hmacSignature, nonce);
            var requestModel = new RequestModel(requestConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        public async Task<bool> DeleteAsync(string webHookId)
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

        public T Create<T>(WebHook webHook)
        {
            return CreateAsync<T>(webHook).GetAwaiter().GetResult();
        }

        public async Task<string> CreateAsync(WebHook webHook)
        {
            return await CreateAsync<string>(webHook);
        }

        public string Create(WebHook webHook)
        {
            return Create<string>(webHook);
        }

        public T Get<T>()
        {
            return GetAsync<T>().GetAwaiter().GetResult();
        }

        public string Get()
        {
            return GetAsync<string>().GetAwaiter().GetResult();
        }

        public bool Delete(string webHookId)
        {
            return DeleteAsync(webHookId).GetAwaiter().GetResult();
        }
    }
}
