using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace APITEST
{
    public static class HttpClientHelper2
    {
        private static readonly HttpClient _httpClient = new HttpClient();

        /// <summary>
        /// 设置全局 Token 认证
        /// </summary>
        /// <param name="token">Token 值</param>
        public static void SetToken(string token)
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        /// <summary>
        /// 发送 GET 请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <returns>泛型结果</returns>
        public static async Task<T> GetAsync<T>(string url)
        {
            try
            {
                var response = await _httpClient.GetAsync(url);
                response.EnsureSuccessStatusCode();
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"GET 请求异常: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 发送 POST 请求
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据（JSON 格式）</param>
        /// <returns>泛型结果</returns>
        public static async Task<T> PostAsync<T>(string url, object data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST 请求异常: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 发送 POST 请求（带自定义 Headers）
        /// </summary>
        /// <typeparam name="T">返回类型</typeparam>
        /// <param name="url">请求地址</param>
        /// <param name="data">请求数据（JSON 格式）</param>
        /// <param name="headers">自定义 Headers</param>
        /// <returns>泛型结果</returns>
        public static async Task<T> PostAsync<T>(string url, object data, HttpHeaders headers)
        {
            try
            {
                _httpClient.DefaultRequestHeaders.Clear();
                foreach (var header in headers)
                {
                    _httpClient.DefaultRequestHeaders.Add(header.Key, header.Value);
                }

                var json = JsonSerializer.Serialize(data);
                var content = new StringContent(json, Encoding.UTF8, "application/json");
                var response = await _httpClient.PostAsync(url, content);
                response.EnsureSuccessStatusCode();
                var result = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(result);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"POST 请求异常（带 Headers）: {ex.Message}");
                throw;
            }
        }

        /// <summary>
        /// 将对象转换为 JSON 字符串
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="obj">对象</param>
        /// <returns>JSON 字符串</returns>
        public static string ToJson<T>(T obj)
        {
            return JsonSerializer.Serialize(obj);
        }

        /// <summary>
        /// 将 JSON 字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="json">JSON 字符串</param>
        /// <returns>对象</returns>
        public static T FromJson<T>(string json)
        {
            return JsonSerializer.Deserialize<T>(json);
        }
    }
}
