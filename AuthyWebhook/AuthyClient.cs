using AuthyWebhook.Models;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyClient
    {
        private readonly AuthyConfiguration configuration;
        private HttpClient Client;
        public AuthyClient(AuthyConfiguration configuration)
        {
            Client = new HttpClient();
            this.configuration = configuration;
        }
        public async Task<string> SendHttpRequest(RequestModel request)
        {
            string response = "";
            var requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", request.CallbackUrl),
                new KeyValuePair<string, string>("name", request.Name),
                new KeyValuePair<string, string>("events[]", request.EventName),
                new KeyValuePair<string, string>("app_api_key", configuration.ApiKey),
                new KeyValuePair<string, string>("access_key", configuration.AccessKey),
            });

            Client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", request.Nonce);
            Client.DefaultRequestHeaders.Add("X-Authy-Signature", request.HmacSignature);
            var result = await Client.PostAsync(Constants.AUTHY_WEBHOOK_URL, requestContent);
            response = await result.Content.ReadAsStringAsync();

            return response;
        }
    }
}
