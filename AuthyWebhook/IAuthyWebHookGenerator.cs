using System.Threading.Tasks;
using AuthyWebhook.Models;

namespace AuthyWebhook
{
    public interface IAuthyWebHookGenerator
    {
        Task<string> CreateWebhooksAsync(WebHookConfiguration webHookConfiguration);
        string CreateWebhooks(WebHookConfiguration webHookConfiguration);
    }
}
