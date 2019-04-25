using System;

namespace AuthyWebhook.Console
{
    class Program
    {
        static void Main(string[] args)
        {
            AuthyWebHookGenerator generator = new AuthyWebHookGenerator();
            generator.CreateWebhooks().GetAwaiter().GetResult();
        }
    }
}
