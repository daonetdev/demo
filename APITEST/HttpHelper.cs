using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APITEST
{
    public static class HttpClientHelper
    {
        /// <summary>
        /// Get
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="parameters"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecuteGet<TResult>(string url, string token, IDictionary<string, string> parameters = null)
        {

            using (var client = new HttpClient())
            {
                // 设置Bearer认证头

                if (!string.IsNullOrWhiteSpace(token))
                    client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);


                // 添加查询参数
                if (parameters != null)
                {
                    var queryBuilder = new StringBuilder();
                    foreach (var param in parameters)
                    {
                        if (queryBuilder.Length > 0) queryBuilder.Append("&");
                        queryBuilder.Append($"{Uri.EscapeDataString(param.Key)}={Uri.EscapeDataString(param.Value)}");
                    }

                    url += $"?{queryBuilder}";
                }

                // 发送请求
                var response = await client.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(content);
            }
        }


        public static async Task Bearer(string token, string url)
        {
            // HttpClientHandler及其派生类使开发人员能够配置各种选项, 包括从代理到身份验证。
            // helpLink https://docs.microsoft.com/en-us/dotnet/api/system.net.http.httpclienthandler?view=netframework-4.8
            var httpclientHandler = new HttpClientHandler();

            // 如果服务器有 https 证书，但是证书不安全，则需要使用下面语句
            // => 也就是说，不校验证书，直接允许
            httpclientHandler.ServerCertificateCustomValidationCallback = (message, cert, chain, error) => true;

            using (var httpClient = new HttpClient(httpclientHandler))
            {
                // 创建身份认证
                // System.Net.Http.Headers.AuthenticationHeaderValue;
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
                await httpClient.GetAsync(url);
                httpClient.Dispose();
            }
        }

        /// <summary>
        /// POST Need Authorization
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecutePostAuth<TResult>(string url, string token, string data)
        {
            using (var client = new HttpClient())
            {
                var request = new HttpRequestMessage
                {
                    Method = HttpMethod.Post,
                    RequestUri = new Uri(url),
                    Headers =
                    {
                        { "Accept", "*/*" },
                        { "User-Agent", "EchoapiRuntime/1.1.0" },
                        { "Connection", "keep-alive" },
                        { "Authorization", $"Bearer {token}" },
                    },
                    Content = new StringContent(data)
                    { Headers = { ContentType = new MediaTypeHeaderValue("application/json") } }
                };
                using (var response = await client.SendAsync(request))
                {
                    response.EnsureSuccessStatusCode();
                    var body = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<TResult>(body);
                }
            }
        }


        /// <summary>
        /// POST No Authorization
        /// </summary>
        /// <typeparam name="TResult"></typeparam>
        /// <param name="url"></param>
        /// <param name="token"></param>
        /// <param name="data"></param>
        /// <returns></returns>
        public static async Task<TResult> ExecutePostNoAuth<TResult>(string url, string data = null)
        {
            using (var client = new HttpClient())
            {
                // 序列化请求体
                var jsonContent = JsonConvert.SerializeObject(data);
                var content = new StringContent(jsonContent, Encoding.UTF8, "application/json");

                // 发送POST请求
                var response = await client.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonConvert.DeserializeObject<TResult>(responseContent);
            }
        }
    }
}
