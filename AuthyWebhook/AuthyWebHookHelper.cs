using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
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

        /// <summary>
        /// Create new Webhook in asyc way
        /// </summary>
        /// <typeparam name="T">Response or string</typeparam>
        /// <param name="webHook">Webhook details</param>
        /// <returns>Newly created webhook in T format</returns>
        public async Task<T> CreateAsync<T>(WebHook webHook)
        {
            string dataToSign = GetDataToSign(webHook);

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var cryptoConfiguration = new CryptoConfiguration(hmacSignature, nonce);
            var requestModel = new Request(webHook, cryptoConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        /// <summary>
        /// Get all registered webhooks in asyc way
        /// </summary>
        /// <typeparam name="T">IList<Response> or string</typeparam>
        /// <returns>List of registered webhooks in T format</returns>
        public async Task<T> GetAsync<T>()
        {
            var webHook = new WebHook();

            string dataToSign = GetDataToSign(webHook);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var cryptoConfiguration = new CryptoConfiguration(hmacSignature, nonce);
            var requestModel = new Request(cryptoConfiguration);
            var authyClient = new AuthyClient(configuration);
            return await authyClient.SendHttpRequest<T>(requestModel);
        }

        /// <summary>
        /// Delete already exist webhook in asyc way
        /// </summary>
        /// <param name="webHookId">Webhook Id what you want to delete</param>
        /// <returns>Action result</returns>
        public async Task<bool> DeleteAsync(string webHookId)
        {
            var webHook = new WebHook(webHookId);

            string dataToSign = GetDataToSign(webHook);
            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            var cryptoConfiguration = new CryptoConfiguration(hmacSignature, nonce);
            var requestModel = new Request(webHook, cryptoConfiguration);
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

            return $"{nonce}|{webHook.RequestType}|{webHook.GetAuthyUrl()}|{sortedParams}"; ;
        }

        /// <summary>
        /// Create new Webhook
        /// </summary>
        /// <typeparam name="T">Response or string</typeparam>
        /// <param name="webHook">Webhook details</param>
        /// <returns>Newly created webhook in T format</returns>
        public T Create<T>(WebHook webHook)
        {
            return CreateAsync<T>(webHook).GetAwaiter().GetResult();
        }

        /// <summary>
        /// Create new Webhook in asyc way
        /// </summary>
        /// <param name="webHook">Webhook details</param>
        /// <returns>Newly created webhook in string format</returns>
        public async Task<string> CreateAsync(WebHook webHook)
        {
            return await CreateAsync<string>(webHook);
        }

        /// <summary>
        /// Create new Webhook
        /// </summary>
        /// <param name="webHook">Webhook details</param>
        /// <returns>Newly created webhook in string format</returns>
        public string Create(WebHook webHook)
        {
            return Create<string>(webHook);
        }

        /// <summary>
        /// Get all registered webhooks
        /// </summary>
        /// <typeparam name="T">IList<Response> or string</typeparam>
        /// <returns>List of registered webhooks in T format</returns>
        public T Get<T>()
        {
            return GetAsync<T>().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Get all registered webhooks
        /// </summary>
        /// <returns>List of registered webhooks in string format</returns>
        public string Get()
        {
            return GetAsync<string>().GetAwaiter().GetResult();
        }

        /// <summary>
        /// Delete already exist webhook
        /// </summary>
        /// <param name="webHookId">Webhook Id what you want to delete</param>
        /// <returns>Action result</returns>
        public bool Delete(string webHookId)
        {
            return DeleteAsync(webHookId).GetAwaiter().GetResult();
        }
    }
}
