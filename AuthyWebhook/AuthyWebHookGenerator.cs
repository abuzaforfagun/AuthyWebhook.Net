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
            dataToSign =
                "213|POST|https://api.authy.com/dashboard/json/application/webhooks|0=a&1=c&10=%3D&100=e&101=v&102=e&103=n&104=t&105=s&106=%5B&107=%5D&108=%3D&109=o&11=1&110=n&111=e&112=_&113=t&114=o&115=W&116=u&117=c&118=h&119=_&12=V&120=r&121=e&122=q&123=u&124=e&125=s&126=t&127=_&128=r&129=e&13=i&130=s&131=p&132=o&133=n&134=d&135=e&136=d&137=%26&138=n&139=a&14=k&140=m&141=e&142=%3D&143=o&144=n&145=e&146=_&147=t&148=o&149=u&15=d&150=c&151=h&152=_&153=r&154=e&155=q&156=u&157=e&158=s&159=t&16=x&160=_&161=r&162=e&163=s&164=p&165=o&166=n&167=d&168=e&169=d&17=y&170=%26&171=u&172=r&173=l&174=%3D&175=h&176=t&177=t&178=p&179=s&18=d&180=%3A&181=%2F&182=%2F&183=e&184=x&185=a&186=m&187=p&188=l&189=e&19=e&190=%2F&191=a&192=p&193=i&194=%2F&195=w&196=e&197=b&198=h&199=o&2=c&20=R&200=o&201=k&202=e&203=d&21=K&22=O&23=J&24=4&25=Z&26=B&27=F&28=Q&29=Q&3=e&30=U&31=D&32=k&33=q&34=4&35=3&36=p&37=T&38=v&39=e&4=s&40=e&41=r&42=X&43=t&44=d&45=h&46=F&47=l&48=k&49=V&5=s&50=L&51=d&52=3&53=Y&54=%26&55=a&56=p&57=p&58=_&59=a&6=_&60=p&61=i&62=_&63=k&64=e&65=y&66=%3D&67=h&68=x&69=e&7=k&70=p&71=M&72=v&73=9&74=d&75=q&76=M&77=4&78=2&79=q&8=e&80=4&81=7&82=l&83=L&84=I&85=7&86=k&87=M&88=G&89=0&9=y&90=F&91=J&92=T&93=9&94=W&95=B&96=1&97=O&98=k&99=%26&access_key=1VikdxydeRKOJ4ZBFQQUDkq43pTveerXtdhFlkVLd3Y&app_api_key=hxepMv9dqM42q47lLI7kMG0FJT9WB1Ok";


            //var sha = GenerateHMACSHA256Hash(dataToSign, signInKey);
            var computed_sig = GenerateHmac(dataToSign, signInKey);
            var new_sha = Convert.ToBase64String(UnicodeEncoding.UTF8.GetBytes(computed_sig));


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

        }

        string GenerateHmac(string message, string key)
        {
            var encoding = new System.Text.ASCIIEncoding();

            using (var hmacsha256 = new HMACSHA256(encoding.GetBytes(key)))

            {
                byte[] hashmessage = hmacsha256.ComputeHash(encoding.GetBytes(message));

                var base64String = Convert.ToBase64String(hashmessage);

                return base64String;

            }
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
