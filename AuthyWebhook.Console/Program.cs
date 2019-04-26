﻿using System.Collections.Generic;
using AuthyWebhook.Models;

namespace AuthyWebhook.Console
{
    class Program
    {
        private static AuthyWebHookHelper authyWebHookHelper;
        static void Main(string[] args)
        {
            var authyConfiguration = new AuthyConfiguration("API_KEY", "ACCESS_KEY", "SIGNIN_KEY");
            authyWebHookHelper = new AuthyWebHookHelper(authyConfiguration);

            var result = ListSample <IList<Response>>();
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
