using System;
using Newtonsoft.Json;

namespace AuthyWebhook.Models
{
    public class ResponseWebHook
    {
        public string id { get; set; }
        public string name { get; set; }
        public string account_sid { get; set; }
        public string service_id { get; set; }
        public string url { get; set; }
        public string signing_key { get; set; }
        public string[] events { get; set; }
        public DateTime creation_date { get; set; }

    }
    public class Response
    {
        public ResponseWebHook webhook { get; set; }
        public string message { get; set; }
        public string success { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
