using AuthyWebhook.Models;

namespace AuthyWebhook.Console
{
    class Program
    {
        private static AuthyWebHookHelper authyWebHookHelper;
        static void Main(string[] args)
        {
            var authyConfiguration = new AuthyConfiguration("hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok", "1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y", "sMDyDH5Z3tWfg8Z3dG44nJ2kg9Gsc48O");
            authyWebHookHelper = new AuthyWebHookHelper(authyConfiguration);

            var result = ListSample <ResponseList>();
            System.Console.WriteLine(result);
            System.Console.Read();
        }

        static T CreateSample<T>()
        {
            var webHook = new WebHook("one_touch_request_responded", "one_touch_request_responded", "https://example/api/webhooked");
            return authyWebHookHelper.Create<T>(webHook);
        }

        static T ListSample<T>()
        {
            return authyWebHookHelper.Get<T>();
        }

        static bool DeleteSample<T>()
        {
            return authyWebHookHelper.Delete("WEB_HOOK_ID");
        }
    }
}
