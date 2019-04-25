using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator
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
            string sortedParams =
                $"access_key={configuration.AccessKey}&app_api_key={configuration.ApiKey}&events%5B%5D={webHookConfiguration.EventName}&name={webHookConfiguration.Name}&url={Uri.EscapeDataString(webHookConfiguration.CallBackUrl)}";

            string dataToSign = $"{nonce}|{method}|{_url}|{sortedParams}";

            var computed_sig = cryptographyHelper.GenerateHmac(dataToSign, configuration.SigninKey);
            
            return await SendHttpRequest(webHookConfiguration.CallBackUrl, webHookConfiguration.Name, webHookConfiguration.EventName, computed_sig);

        }

        public string CreateWebhooks(WebHookConfiguration webHookConfiguration)
        {
            return CreateWebhooksAsync(webHookConfiguration).GetAwaiter().GetResult();
        }

        private async Task<string> SendHttpRequest(string callBackUrl, string name, string events, string computed_sig)
        {
            HttpClient client = new HttpClient();
            string response = "";
            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", callBackUrl),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("events[]", events),
                new KeyValuePair<string, string>("app_api_key", configuration.ApiKey),
                new KeyValuePair<string, string>("access_key", configuration.AccessKey),
            });

            client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-Authy-Signature", computed_sig);
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
