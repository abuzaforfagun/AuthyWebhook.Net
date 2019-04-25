using HashLib;
using Sodium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator
    {
        private string BaseUrl;

        public AuthyWebHookGenerator()
        {
            BaseUrl = "https://api.authy.com";
        }
        public async Task CreateWebhooks()
        {
            string url = BaseUrl + "/dashboard/json/application/webhooks";
            string[] events = { "one_touch_request_responded" };

            string method = "POST";
            string apiKey = "hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok";
            string accessKey = "1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y";
            string signInKey = "sMDyDH5Z3tWfg8Z3dG44nJ2kg9Gsc48O";
            string callBackUrl = "https://example/api/webhooked";
            string name = "one_touch_request_responded";
            string nonce = "2134123";
            string sortedParams =
                $"access_key={accessKey}&app_api_key={apiKey}&events%5B%5D={events[0]}&name={name}&url={Uri.EscapeDataString(callBackUrl)}";

            string dataToSign = $"{nonce}|{method}|{url}|{sortedParams}";

            var computed_sig = GenerateHmac(dataToSign, signInKey);
            

            HttpClient client = new HttpClient();
            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", callBackUrl),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("events[]", events[0]),
                new KeyValuePair<string, string>("app_api_key", apiKey),
                new KeyValuePair<string, string>("access_key", accessKey),
            });
            
            client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-Authy-Signature", computed_sig);
            try
            {
                HttpResponseMessage result = client.PostAsync(url, requestContent).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }

        }

        string GenerateHmac(string message, string key)
        {
            var encoding = new System.Text.ASCIIEncoding();

            using (var hmacsha256 = new HMACSHA256(Encoding.ASCII.GetBytes(key)))

            {
                byte[] hashmessage = hmacsha256.ComputeHash(Encoding.ASCII.GetBytes(message));

                var base64String = Convert.ToBase64String(hashmessage);

                return base64String;

            }
        }
    }
}
