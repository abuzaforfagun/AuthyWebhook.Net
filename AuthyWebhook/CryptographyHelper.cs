using System;
using System.Security.Cryptography;
using System.Text;

namespace AuthyWebhook
{
    public class CryptographyHelper : ICryptographyHelper
    {
        public string GenerateHmac(string message, string key)
        {
            var encoding = new ASCIIEncoding();

            using (var hmacsha256 = new HMACSHA256(Encoding.ASCII.GetBytes(key)))

            {
                byte[] hashmessage = hmacsha256.ComputeHash(Encoding.ASCII.GetBytes(message));

                var base64String = Convert.ToBase64String(hashmessage);

                return base64String;

            }
        }
    }
}
