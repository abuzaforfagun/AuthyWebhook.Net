using HashLib;
using Sodium;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AuthyWebhook
{
    public class AuthyWebHookGenerator
    {
        private string BaseUrl;

        public AuthyWebHookGenerator()
        {
            BaseUrl = "https://api.authy.com";
        }
        public async Task CreateWebhooks()
        {
            string url = BaseUrl + "/dashboard/json/application/webhooks";
            string[] events = { "one_toWuch_request_responded" };

            string method = "POST";
            string apiKey = "hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok";
            string accessKey = "1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y";
            string signInKey = "sMDyDH5Z3tWfg8Z3dG44nJ2kg9Gsc48O";
            string callBackUrl = "https://example/api/webhooked";
            string name = "one_touch_request_responded";
            string nonce = "213";
            string sortedParams =
                $"access_key={accessKey}&app_api_key={apiKey}&events[]={events[0]}&name={name}&url={callBackUrl}";
            //sortedParams = Uri.EscapeDataString(sortedParams);


            string dataToSign = $"{nonce}|{method}|{url}|{sortedParams}";


            var computed_sig = GenerateHMACSHA256Hash(dataToSign, signInKey);
            
            HttpClient client = new HttpClient();
            FormUrlEncodedContent requestContent = new FormUrlEncodedContent(new[] {
                new KeyValuePair<string, string>("url", callBackUrl),
                new KeyValuePair<string, string>("name", name),
                new KeyValuePair<string, string>("events[]", events[0]),
                new KeyValuePair<string, string>("app_api_key", apiKey),
                new KeyValuePair<string, string>("access_key", accessKey),
            });
            //computed_sig = "j781eqrHy44ilJW+ezReaRHj/C9stNNNt9HyojIOBh8=";
            client.DefaultRequestHeaders.Add("X-Authy-Signature-Nonce", nonce);
            client.DefaultRequestHeaders.Add("X-Authy-Signature", computed_sig);
            try
            {
                HttpResponseMessage result = client.PostAsync(url, requestContent).GetAwaiter().GetResult();
            }
            catch (Exception ex)
            {

            }
            //anotherDemo();

        }



        string Encrypt(string source, string key)
        {
            TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

            byte[] byteBuff;

            try
            {
                desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
                byteBuff = Encoding.UTF8.GetBytes(source);

                string encoded =
                    Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

                return encoded;
            }
            catch (Exception except)
            {
                Console.WriteLine(except + "\n\n" + except.StackTrace);
                return null;
            }
        }







        byte[] HmacSha256(byte[] key, string data)
        {
            using (var hmac = new HMACSHA256(key))
            {
                return hmac.ComputeHash(Encoding.UTF8.GetBytes(data));
            }
        }

        string GenerateHMACSHA256Hash(string stringValue, string key)
        {
            Encoding encoding = Encoding.UTF8;
            var hash = GenerateHMACSHA256HashBytes(stringValue, key);
            var hex = hash.Select<byte, string>(a => a.ToString("x2"))
                .Aggregate<string>((a, b) => string.Format("{0}{1}", a, b));
            return hex;
        }


        public static byte[] GenerateHMACSHA256HashBytes(string message, string key)
        {
            Encoding encoding = Encoding.UTF8;

            //Reference http://en.wikipedia.org/wiki/Secure_Hash_Algorithm
            //SHA256 block size is 512 bits => 64 bytes.
            const int HashBlockSize = 64;


            var keyBytes = encoding.GetBytes(key);
            var opadKeySet = new byte[HashBlockSize];
            var ipadKeySet = new byte[HashBlockSize];


            if (keyBytes.Length > HashBlockSize)
            {
                keyBytes = GetHash(keyBytes);
            }

            // This condition is independent of previous
            // condition. If previous was true
            // we still need to execute this to make keyBytes same length
            // as blocksize with 0 padded if its less than block size
            if (keyBytes.Length < HashBlockSize)
            {
                var newKeyBytes = new byte[HashBlockSize];
                keyBytes.CopyTo(newKeyBytes, 0);
                keyBytes = newKeyBytes;
            }


            for (int i = 0; i < keyBytes.Length; i++)
            {
                opadKeySet[i] = (byte)(keyBytes[i] ^ 0x5C);
                ipadKeySet[i] = (byte)(keyBytes[i] ^ 0x36);
            }

            var hash = GetHash(ByteConcat(opadKeySet,
                GetHash(ByteConcat(ipadKeySet, encoding.GetBytes(message)))));
            return hash;
        }

        public static byte[] GetHash(byte[] bytes)
        {
            var sha256Digest = new Org.BouncyCastle.Crypto.Digests.Sha256Digest();
            sha256Digest.BlockUpdate(bytes, 0, bytes.Length);
            byte[] result = new byte[sha256Digest.GetDigestSize()];
            sha256Digest.DoFinal(result, 0);
            return result;
        }

        public static byte[] ByteConcat(byte[] left, byte[] right)
        {
            if (null == left)
            {
                return right;
            }

            if (null == right)
            {
                return left;
            }

            byte[] newBytes = new byte[left.Length + right.Length];
            left.CopyTo(newBytes, 0);
            right.CopyTo(newBytes, left.Length);

            return newBytes;
        }

        private string WithLibsodium(string message, string key)
        {
            //key = key.Replace('-', '+').Replace('_', '/').PadRight(key.Length + (4 - key.Length % 4) % 4, '=');
            key = key.Replace(System.Environment.NewLine, "");
            var secretBase64Decoded = Convert.FromBase64String(key);
            var hmac = Convert.ToBase64String(HmacSha256(secretBase64Decoded, message));
            return hmac;

            //var _key = UnicodeEncoding.UTF32.GetBytes(key);
            //HMACSHA256 hmac = new HMACSHA256(_key);
            //var data = hmac.ComputeHash(UnicodeEncoding.UTF32.GetBytes(message));
            //byte[] data = null;
            ////returns a 32 byte authentication code
            //try
            //{
            //    data = SecretKeyAuth.SignHmacSha256(message, _key);

            //}
            //catch (Exception ex)
            //{

            //}
            //var signature = SecretKeyAuth.Sign(message, Encoding.ASCII.GetBytes(key));

            //return Convert.ToBase64String(data);
        }
        private string EncryptWithHashlib(string message, string key)
        {

            IHMAC hmac = HashFactory.HMAC.CreateHMAC(HashFactory.Crypto.CreateSHA256());
            hmac.Key = Converters.ConvertStringToBytes(key, Encoding.UTF32);
            var r = hmac.ComputeString(message, Encoding.UTF32);
            return Convert.ToBase64String(r.GetBytes());
            //return r.ToString();

        }
        //private string HashHMAC(string _message, string _key)
        //{
        //    var key = Encoding.ASCII.GetBytes(_key);
        //    var message = Encoding.ASCII.GetBytes(_message);
        //    var hash = new HMACSHA256(key);
        //    var hashValue = hash.ComputeHash(message);
        //    return Convert.ToBase64String(hashValue);
        //}

        //private string CreateToken(string message, string secret)
        //{
        //    secret = secret ?? "";
        //    var encoding = new System.Text.ASCIIEncoding();
        //    byte[] keyByte = encoding.GetBytes(secret);
        //    byte[] messageBytes = encoding.GetBytes(message);
        //    using (var hmacsha256 = new HMACSHA256(keyByte))
        //    {
        //        byte[] hashmessage = hmacsha256.ComputeHash(messageBytes);
        //        return Convert.ToBase64String(hashmessage);
        //    }
        //}

        //String GetHash(String text, String key)
        //{
        //    // change according to your needs, an UTF8Encoding
        //    // could be more suitable in certain situations
        //    ASCIIEncoding encoding = new ASCIIEncoding();

        //    Byte[] textBytes = encoding.GetBytes(text);
        //    Byte[] keyBytes = encoding.GetBytes(key);

        //    Byte[] hashBytes;

        //    using (HMACSHA256 hash = new HMACSHA256(keyBytes))
        //        hashBytes = hash.ComputeHash(textBytes);

        //    return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
        //}

        //public string Encode(string input, string _key)
        //{
        //    var key = Encoding.ASCII.GetBytes(_key);
        //    HMACSHA1 myhmacsha1 = new HMACSHA1(key);
        //    byte[] byteArray = Encoding.ASCII.GetBytes(input);
        //    MemoryStream stream = new MemoryStream(byteArray);
        //    return myhmacsha1.ComputeHash(stream).Aggregate("", (s, e) => s + String.Format("{0:x2}", e), s => s);
        //}


        //public string Encrypt(string source, string key)
        //{
        //    TripleDESCryptoServiceProvider desCryptoProvider = new TripleDESCryptoServiceProvider();

        //    byte[] byteBuff;

        //    try
        //    {
        //        desCryptoProvider.Key = Encoding.UTF8.GetBytes(key);
        //        desCryptoProvider.IV = UTF8Encoding.UTF8.GetBytes("ABCDEFGH");
        //        byteBuff = Encoding.UTF8.GetBytes(source);

        //        string iv = Convert.ToBase64String(desCryptoProvider.IV);
        //        Console.WriteLine("iv: {0}", iv);

        //        string encoded =
        //            Convert.ToBase64String(desCryptoProvider.CreateEncryptor().TransformFinalBlock(byteBuff, 0, byteBuff.Length));

        //        return encoded;
        //    }
        //    catch (Exception except)
        //    {
        //        Console.WriteLine(except + "\n\n" + except.StackTrace);
        //        return null;
        //    }
        //}


        public string Encrypt2(string plainText, string passPhrase)
        {
            // Salt and IV is randomly generated each time, but is preprended to encrypted cipher text
            // so that the same Salt and IV values can be used when decrypting.  
            int DerivationIterations = 1000;
            int Keysize = 256;
            var saltStringBytes = Generate256BitsOfRandomEntropy();
            var ivStringBytes = Generate256BitsOfRandomEntropy();
            var plainTextBytes = Encoding.UTF8.GetBytes(plainText);
            using (var password = new Rfc2898DeriveBytes(Encoding.UTF8.GetBytes(passPhrase), saltStringBytes, DerivationIterations))
            {
                var keyBytes = password.GetBytes(Keysize / 8);
                using (var symmetricKey = new RijndaelManaged())
                {
                    symmetricKey.BlockSize = 256;
                    symmetricKey.Mode = CipherMode.CBC;
                    symmetricKey.Padding = PaddingMode.PKCS7;
                    using (var encryptor = symmetricKey.CreateEncryptor(keyBytes, ivStringBytes))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            using (var cryptoStream = new CryptoStream(memoryStream, encryptor, CryptoStreamMode.Write))
                            {
                                cryptoStream.Write(plainTextBytes, 0, plainTextBytes.Length);
                                cryptoStream.FlushFinalBlock();
                                // Create the final bytes as a concatenation of the random salt bytes, the random iv bytes and the cipher bytes.
                                var cipherTextBytes = saltStringBytes;
                                cipherTextBytes = cipherTextBytes.Concat(ivStringBytes).ToArray();
                                cipherTextBytes = cipherTextBytes.Concat(memoryStream.ToArray()).ToArray();
                                memoryStream.Close();
                                cryptoStream.Close();
                                return Convert.ToBase64String(cipherTextBytes);
                            }
                        }
                    }
                }
            }
        }

        string EncryptWithRij(string original, string key)
        {
            using (RijndaelManaged myRijndael = new RijndaelManaged())
            {

                myRijndael.Key = Encoding.ASCII.GetBytes(key);
                myRijndael.GenerateIV();
                // Encrypt the string to an array of bytes.
                byte[] encrypted = EncryptStringToBytes(original, myRijndael.Key, myRijndael.IV);
                return Convert.ToBase64String(encrypted);
            }
        }

        byte[] EncryptStringToBytes(string plainText, byte[] Key, byte[] IV)
        {
            // Check arguments.
            if (plainText == null || plainText.Length <= 0)
                throw new ArgumentNullException("plainText");
            if (Key == null || Key.Length <= 0)
                throw new ArgumentNullException("Key");
            if (IV == null || IV.Length <= 0)
                throw new ArgumentNullException("IV");
            byte[] encrypted;
            // Create an RijndaelManaged object
            // with the specified key and IV.
            using (RijndaelManaged rijAlg = new RijndaelManaged())
            {
                rijAlg.Key = Key;
                rijAlg.IV = IV;

                // Create an encryptor to perform the stream transform.
                ICryptoTransform encryptor = rijAlg.CreateEncryptor(rijAlg.Key, rijAlg.IV);

                // Create the streams used for encryption.
                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {

                            //Write all data to the stream.
                            swEncrypt.Write(plainText);
                        }
                        encrypted = msEncrypt.ToArray();
                    }
                }
            }


            // Return the encrypted bytes from the memory stream.
            return encrypted;

        }

        private byte[] Generate256BitsOfRandomEntropy()
        {
            var randomBytes = new byte[32]; // 32 Bytes will give us 256 bits.
            using (var rngCsp = new RNGCryptoServiceProvider())
            {
                // Fill the array with cryptographically secure random bytes.
                rngCsp.GetBytes(randomBytes);
            }
            return randomBytes;
        }


        private void Dummy()
        {
            using (var httpClient = new HttpClient())
            {
                using (var request = new HttpRequestMessage(new HttpMethod("POST"), "https://api.authy.com/dashboard/json/application/webhooks"))
                {
                    request.Headers.TryAddWithoutValidation("X-Authy-Signature-Nonce", "155612034586700.155612034586701");
                    request.Headers.TryAddWithoutValidation("X-Authy-Signature", "3cFKMHqi09+DWLNMMzQA/M1y7WZWJjsdVlkbeNSvnnM=");

                    var contentList = new List<string>();
                    contentList.Add("url=\"https://example/api/webhooked\"");
                    contentList.Add("name=\"one_touch_request_responded\"");
                    contentList.Add("events[]=\"one_touch_request_responded\"");
                    contentList.Add("app_api_key=\"hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok\"");
                    contentList.Add("access_key=\"1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y\"");
                    request.Content = new StringContent(string.Join("&", contentList), Encoding.UTF8, "application/x-www-form-urlencoded");

                    var response = httpClient.SendAsync(request).GetAwaiter().GetResult();
                }
            }
        }


    }
}
