using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace APITEST
{
    internal class Program
    {
        static async Task Main(string[] args)
        {

            string sPostUrl = "https://localhost:31059/api/v1/supplier/process/record";
            string sToken = "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJhdWQiOlsib2F1dGgyLXJlc291cmNlIl0sInNjb3BlIjpbImFsbCJdLCJleHAiOjE3NDcyOTkwMjEsImp0aSI6ImU5ZDFhZGM0LThmYTgtNGNlZC1iNDZkLWJjNDkyZTdjYmFlOCIsImNsaWVudF9pZCI6InN1cHBsaWVyXzAwMDAwMDIxOTIifQ.N7QfySmKtKWK98PyIUo0MpPCIawgiQwLiT5qwEKkRnJdG1m0vg-xjKXXoFn0lJMkLXMOjnRuRt_JPwYWg1mRrOnsvonOGBtU3IlMaDb3ckyhpXoXNgoUEODk4hWdmxS2JoLs3jjjajwPa7RcO-NPILwtOJa5QqI0Uzswk5Ce0-RwYydC1AZMlW6_VGijZhVA5Rrpfqyb4IBTNaOuzhImkCJKQBHFqUK1AFdbW5t76VqiuYdCC29OD-xyeqtqxlJ-HJmIKdsJ7pcJ2u3PNNjIb70uQYGT3tn3bdOQn5MeyXWPPWpKMZw1CCPWtou-bG8tnlMFTXNdq983_1rLHhsFTA";
            string sPostData = "[{\"bizId\":\"29B6A118AA284812B315F1D58E9089\",\"supplierProductCode\":\"A1101100011\",\"supplierProductName\":\"AFM-03-W-S11\",\"partCode\":\"4134010-RR01\",\"partName\":\"AFM-03-W-S11\",\"factory\":\"芜湖安瑞光电有限公司\",\"model\":\"TEST\",\"productionLine\":\"组装1线\",\"process\":\"1组装\",\"processOrder\":\"1\",\"station\":\"1组装001\",\"stationOrder\":\"1\",\"device\":\"1组装001\",\"supplierSerialNumber\":\"WO24082200010003\",\"serialNumber\":\"219241340100051S4500045\",\"lotCode\":\"S45\",\"isRetest\":false,\"testNumber\":\"1\",\"testStartTime\":1747102867136,\"testEndTime\":1747102867137,\"stationEnterTime\":1747102867036,\"stationOutTime\":1747102867237,\"testResult\":\"1\"},{\"bizId\":\"979B35EDAC6E455496231032B54437\",\"supplierProductCode\":\"A1101100011\",\"supplierProductName\":\"AFM-03-W-S11\",\"partCode\":\"4134010-RR01\",\"partName\":\"AFM-03-W-S11\",\"factory\":\"芜湖安瑞光电有限公司\",\"model\":\"TEST\",\"productionLine\":\"组装1线\",\"process\":\"1组装\",\"processOrder\":\"1\",\"station\":\"1组装001\",\"stationOrder\":\"1\",\"device\":\"1组装001\",\"supplierSerialNumber\":\"WO24082200010004\",\"serialNumber\":\"219241340100051S4500046\",\"lotCode\":\"S45\",\"isRetest\":false,\"testNumber\":\"1\",\"testStartTime\":1747102867136,\"testEndTime\":1747102867137,\"stationEnterTime\":1747102867036,\"stationOutTime\":1747102867237,\"testResult\":\"1\"}]";
            var result = await HttpClientHelper.ExecutePostAuth<PostResult>(sPostUrl, sToken, sPostData);
            string sJson = JsonJavaScriptSerializer.ToJSON(result);
            Console.WriteLine(sJson);
            Console.ReadKey();
        }
    }
}
