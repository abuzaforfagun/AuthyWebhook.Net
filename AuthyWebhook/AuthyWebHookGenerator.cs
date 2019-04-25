using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator : IAuthyWebHookGenerator
    {
        private string _url = "https://api.authy.com/dashboard/json/application/webhooks";
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

            string dataToSign = $"{nonce}|{method}|{_url}|{sortedParams}";

            var hmacSignature = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            
            var requestModel = new RequestModel(webHookConfiguration, hmacSignature);
            return await SendHttpRequest(requestModel);

        }

        public string CreateWebhooks(WebHookConfiguration webHookConfiguration)
        {
            return CreateWebhooksAsync(webHookConfiguration).GetAwaiter().GetResult();
        }

        private async Task<string> SendHttpRequest(RequestModel request)
        {
            HttpClient client = new HttpClient();
            string response = "";
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", request.CallbackUrl),
                new KeyValuePair<string, string>("name", request.Name),
                new KeyValuePair<string, string>("events[]", request.EventName),
                new KeyValuePair<string, string>("app_api_key", configuration.ApiKey),
                new KeyValuePair<string, string>("access_key", configuration.AccessKey),
            });

            client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-Authy-Signature", request.HmacSignature);
            try
            {
                var result = await client.PostAsync(_url, requestContent);
                response = await result.Content.ReadAsStringAsync();
            }
            catch (Exception ex)
            {

            }

            return response;
        }
        
    }
}
