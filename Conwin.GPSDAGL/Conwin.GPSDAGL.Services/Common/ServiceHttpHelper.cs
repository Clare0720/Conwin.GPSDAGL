using Conwin.Framework.CommunicationProtocol;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;

namespace Conwin.GPSDAGL.Services.Common
{
    public class ServiceHttpHelper
    {
        private static readonly HttpClient _client;

        static ServiceHttpHelper()
        {
            _client = new HttpClient();

            _client.DefaultRequestHeaders.Connection.Add("keep-alive");
            _client.DefaultRequestHeaders.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("*/*"));

        }

        public string Post(string url, CWRequest request)
        {
            string response = string.Empty;
            HttpRequestMessage ms = new HttpRequestMessage();
            ms.Content = new StringContent("=" + System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(request)), Encoding.UTF8, "application/x-www-form-urlencoded");
            var token = string.Empty;
            if (System.Web.HttpContext.Current != null && System.Web.HttpContext.Current.Request != null)
                token = System.Web.HttpContext.Current.Request.Headers["token"];
            ms.Headers.Add("token", token);
            ms.Headers.Add("Source-Type", "76d5f6283a57b2db");
            ms.Headers.Add("Access-Control-Allow-Origin", "*");
            ms.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
            ms.RequestUri = new Uri(url);
            ms.Method = HttpMethod.Post;
            var result = _client.SendAsync(ms).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
            }
            return response;
        }

        public string Post(string url, CWRequest request, string token = "")
        {
            string response = string.Empty;
            HttpRequestMessage ms = new HttpRequestMessage();
            ms.Content = new StringContent("=" + System.Web.HttpUtility.UrlEncode(JsonConvert.SerializeObject(request)), Encoding.UTF8, "application/x-www-form-urlencoded");
            ms.Headers.Add("token", token);
            ms.Headers.Add("Source-Type", "76d5f6283a57b2db");
            ms.Headers.Add("Access-Control-Allow-Origin", "*");
            ms.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
            ms.RequestUri = new Uri(url);
            ms.Method = HttpMethod.Post;

            var result = _client.SendAsync(ms).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
            }
            return response;
        }

        public string Post(string url, string request)
        {
            string response = string.Empty;
            HttpRequestMessage ms = new HttpRequestMessage();
            ms.Content = new StringContent(request, Encoding.UTF8, "application/json");
            ms.Headers.Add("Access-Control-Allow-Origin", "*");
            ms.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
            ms.RequestUri = new Uri(url);
            ms.Method = HttpMethod.Post;
            var result = _client.SendAsync(ms).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
            }
            return response;
        }

        public string Get(string uri)
        {
            string response = string.Empty;
            HttpRequestMessage ms = new HttpRequestMessage();
            ms.Headers.Add("Access-Control-Allow-Origin", "*");
            ms.Headers.Add("Access-Control-Allow-Headers", "X-Requested-With");
            ms.RequestUri = new Uri(uri);
            ms.Method = HttpMethod.Get;
            var result = _client.SendAsync(ms).Result;
            if (result.IsSuccessStatusCode)
            {
                response = result.Content.ReadAsStringAsync().Result;
            }
            return response;
        }
    }
}
