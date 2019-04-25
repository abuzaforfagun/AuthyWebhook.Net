using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator
    {
        private readonly string _apiKey;
        private readonly string _accessKey;
        private readonly string _signInKey;
        private string _url = "https://api.authy.com/dashboard/json/application/webhooks";
        private ICryptographyHelper cryptographyHelper;
        private readonly  string nonce;


        public AuthyWebHookGenerator(string apiKey, string accessKey, string signInKey)
        {
            _apiKey = apiKey;
            _accessKey = accessKey;
            _signInKey = signInKey;
            nonce = Guid.NewGuid().ToString();

            cryptographyHelper = new CryptographyHelper();
        }

        public async Task CreateWebhooks(string name, string events, string callBackUrl)
        {

            string method = "POST";
            string sortedParams =
                $"access_key={_accessKey}&app_api_key={_apiKey}&events%5B%5D={events}&name={name}&url={Uri.EscapeDataString(callBackUrl)}";

            string dataToSign = $"{nonce}|{method}|{_url}|{sortedParams}";

            var computed_sig = cryptographyHelper.GenerateHmac(dataToSign, _signInKey);
            
            SendHttpRequest(callBackUrl, name, events, computed_sig);

        }

        void SendHttpRequest(string callBackUrl, string name, string events, string computed_sig)
        {
            HttpClient client = new HttpClient();
            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", callBackUrl),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("events[]", events),
                new KeyValuePair<string, string>("app_api_key", _apiKey),
                new KeyValuePair<string, string>("access_key", _accessKey),
            });

            client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-Authy-Signature", computed_sig);
            try
            {
                HttpResponseMessage result = client.PostAsync(_url, requestContent).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
        }
        
    }
}
