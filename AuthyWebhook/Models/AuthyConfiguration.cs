using System;
using System.Collections.Generic;
using System.Text;

namespace AuthyWebhook.Models
{
    public class AuthyConfiguration
    {
        public string ApiKey { get; set; }
        public string AccessKey { get; set; }
        public string SigninKey { get; set; }

        public AuthyConfiguration()
        {
            
        }
        public AuthyConfiguration(string apiKey, string accessKey, string signinKey)
        {
            this.ApiKey = apiKey;
            this.AccessKey = accessKey;
            this.SigninKey = signinKey;
        }
    }
}
