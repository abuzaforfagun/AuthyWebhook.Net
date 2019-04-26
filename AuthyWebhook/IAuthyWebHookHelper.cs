using System.Threading.Tasks;
using AuthyWebhook.Models;

namespace AuthyWebhook
{
    public interface IAuthyWebHookHelper
    {
        Task<T> CreateAsync<T>(WebHook webHook);
        T Create<T>(WebHook webHook);
        Task<string> CreateAsync(WebHook webHook);
        string Create(WebHook webHook);
        Task<T> GetAsync<T>();
        string Get();
        Task<bool> DeleteAsync(string webHookId);
        bool Delete(string webHookId);
    }
}
