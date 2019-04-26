using System.Threading.Tasks;
using AuthyWebhook.Models;

namespace AuthyWebhook
{
    public interface IAuthyWebHookHelper
    {
        Task<T> CreateWebhooksAsync<T>(WebHook webHook);
        T CreateWebhooks<T>(WebHook webHook);
        Task<string> CreateWebhooksAsync(WebHook webHook);
        string CreateWebhooks(WebHook webHook);
        Task<T> GetAuthyWebhooksAsync<T>();
        string GetAuthyWebhooks();
        Task<bool> DeleteWebHookAsync(string webHookId);
        bool DeleteWebHook(string webHookId);
    }
}
