using System.Collections.Generic;
using Newtonsoft.Json;

namespace AuthyWebhook.Models
{
    public class ResponseList
    {
        public List<ResponseWebHook> webhooks { get; set; }
        public string message { get; set; }
        public bool success { get; set; }

        public override string ToString()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}
