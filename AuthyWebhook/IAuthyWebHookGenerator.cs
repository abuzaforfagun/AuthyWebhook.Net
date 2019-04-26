﻿using System.Threading.Tasks;
using AuthyWebhook.Models;

namespace AuthyWebhook
{
    public interface IAuthyWebHookGenerator
    {
        Task<T> CreateWebhooksAsync<T>(WebHookConfiguration webHookConfiguration);
        T CreateWebhooks<T>(WebHookConfiguration webHookConfiguration);
        Task<string> CreateWebhooksAsync(WebHookConfiguration webHookConfiguration);
        string CreateWebhooks(WebHookConfiguration webHookConfiguration);
    }
}
