using AuthyWebhook.Models;

namespace AuthyWebhook.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            var authyConfiguration = new AuthyConfiguration("hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok", "1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y", "sMDyDH5Z3tWfg8Z3dG44nJ2kg9Gsc48O");
            var generator = new AuthyWebHookGenerator(authyConfiguration);
            var webHookConfiguration = new WebHookConfiguration("one_touch_request_responded", "one_touch_request_responded", "https://example/api/webhooked");
            //var result = generator.CreateWebhooks<Response>(webHookConfiguration);
            var list = generator.GetAuthyWebhooks();
            System.Console.WriteLine(list);
            System.Console.Read();
        }
    }
}
