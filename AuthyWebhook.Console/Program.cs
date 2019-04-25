using System;
using AuthyWebhook.Models;

namespace AuthyWebhook.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthyConfiguration authyConfiguration = new AuthyConfiguration("hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok", "1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y", "sMDyDH5Z3tWfg8Z3dG44nJ2kg9Gsc48O");
            AuthyWebHookGenerator generator = new AuthyWebHookGenerator(authyConfiguration);
            WebHookConfiguration webHookConfiguration = new WebHookConfiguration("one_touch_request_responded", "one_touch_request_responded", "https://example/api/webhooked");
            var result = generator.CreateWebhooks(webHookConfiguration);
        }
    }
}
