using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;

namespace AuthyWebhook.Models
{
    public class RequestConfiguration
    {
        public string HmacSignature { get; }
        public string Nonce { get; }
        public HttpMethod RequestType { get; }

        public RequestConfiguration(string hmacSignature, string nonce)
        {
            HmacSignature = hmacSignature;
            Nonce = nonce;
            RequestType = HttpMethod.Get;
        }

        public RequestConfiguration(string hmacSignature, string nonce, HttpMethod requestType)
        {
            HmacSignature = hmacSignature;
            Nonce = nonce;
            RequestType = requestType;
        }
    }
}
