using System.Threading.Tasks;
using AuthyWebhook.Models;

namespace AuthyWebhook
{
    public interface IAuthyWebHookHelper
    {
        Task<T> CreateWebhooksAsync<T>(WebHookConfiguration webHookConfiguration);
        T CreateWebhooks<T>(WebHookConfiguration webHookConfiguration);
        Task<string> CreateWebhooksAsync(WebHookConfiguration webHookConfiguration);
        string CreateWebhooks(WebHookConfiguration webHookConfiguration);
        Task<T> GetAuthyWebhooksAsync<T>();
        string GetAuthyWebhooks();
    }
}
