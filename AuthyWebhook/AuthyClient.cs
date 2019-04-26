﻿using AuthyWebhook.Models;
using Newtonsoft.Json;
using System;
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
        public async Task<T> SendHttpRequest<T>(RequestModel request)
        {
            var requestContent = GetRequestContent(request.WebHook);

            Client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", request.RequestConfiguration.Nonce);
            Client.DefaultRequestHeaders.Add("X-Authy-Signature", request.RequestConfiguration.HmacSignature);
            var httpRequestMessage = new HttpRequestMessage(request.WebHook.RequestType, Constants.GetAuthyUrl(request.WebHook.Id));
            httpRequestMessage.Content = requestContent;
            
            var result = await Client.SendAsync(httpRequestMessage);
            var response = await result.Content.ReadAsStringAsync();
            if (typeof(T) == typeof(string))
            {
                return (T)Convert.ChangeType(response, typeof(T));
            }
            else
            {
                var deserializeObject = JsonConvert.DeserializeObject(response, typeof(Response));
                return (T)Convert.ChangeType(deserializeObject, typeof(T));
            }
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
