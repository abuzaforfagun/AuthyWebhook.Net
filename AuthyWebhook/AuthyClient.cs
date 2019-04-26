using AuthyWebhook.Models;
using System.Collections.Generic;
using System.Net.Http;
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
        public async Task<string> SendHttpRequest(Request request)
        {
            var requestContent = GetRequestContent(request.WebHook);

            Client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", request.CryptoConfiguration.Nonce);
            Client.DefaultRequestHeaders.Add("X-Authy-Signature", request.CryptoConfiguration.HmacSignature);
            var httpRequestMessage = new HttpRequestMessage(request.WebHook.RequestType, request.WebHook.GetAuthyUrl());
            httpRequestMessage.Content = requestContent;
            
            var result = await Client.SendAsync(httpRequestMessage);
            return await result.Content.ReadAsStringAsync();
        }


        private FormUrlEncodedContent GetRequestContent(WebHook webHook)
        {
            List<KeyValuePair<string, string>> bodyData = new List<KeyValuePair<string, string>>();

            bodyData.AddRange(new [] {
                new KeyValuePair<string, string>("app_api_key", configuration.ApiKey),
                new KeyValuePair<string, string>("access_key", configuration.AccessKey)
            });
            if (webHook.RequestType == HttpMethod.Post)
            {
                bodyData.AddRange(new[]
                {
                    new KeyValuePair<string, string>("url", webHook.CallBackUrl),
                    new KeyValuePair<string, string>("name", webHook.Name),
                    new KeyValuePair<string, string>("events[]", webHook.EventName)
                });
            }
            return new FormUrlEncodedContent(bodyData);
        }
    }
}
