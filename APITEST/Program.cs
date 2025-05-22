using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace APITEST
{
    /// <summary>
    /// 测试提交
    /// </summary>
    internal class Program
    {
        static async Task Main(string[] args)
        {
            //string sPostUrl = "https://localhost:31059/api/v1/sup/record";
            //string sToken = "leHAiOjE3NDcyOTkwKk3ckyh_VGijZhsJ7pcJ2u3PNNjIb70uQYGT3tn3bdOQn5MeyXA";
            //string sPostData = "[{\"guid\":\"29B6A118AA284812B315F1D58E9089\",\"sCode\":\"A1101100011\",\"ProductName\":\"W-S11\"}]";
            //var result = await HttpClientHelper.ExecutePostAuth<PostResult>(sPostUrl, sToken, sPostData);
            //string sJson = JsonJavaScriptSerializer.ToJSON(result);
            //Console.WriteLine(sJson);


            //生成jwt SecretKey
            var key = new byte[32]; // 32字节 = 256位，比128位更安全
            using (var rng = new RNGCryptoServiceProvider())
            {
                rng.GetBytes(key);
            }
            var secret = Convert.ToBase64String(key);
            Console.WriteLine(secret);
            var sPWD= Utils.MD5Hash("0B4E7A0E5FE84AD35FB5F95B9CEEAC79");

            Console.WriteLine(sPWD);

            Console.ReadKey();
        }
    }
}
