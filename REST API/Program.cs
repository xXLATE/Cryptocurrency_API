using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;

namespace REST_API
{
    class Program
    {
        static void Main()
        {
            string currency = "dogecoin";
            var url = $"https://api.coincap.io/v2/assets?search={currency}";

            var request = WebRequest.Create(url);

            var response = request.GetResponse();
            var httpStatusCode = (response as HttpWebResponse).StatusCode;

            if (httpStatusCode != HttpStatusCode.OK)
            {
                Console.WriteLine(httpStatusCode);
                return;
            }

            using (var streamReader = new StreamReader(response.GetResponseStream()))
            {
                string result = streamReader.ReadToEnd();
                var coinsForecast = JsonConvert.DeserializeObject<Root>(result);
                Console.WriteLine(coinsForecast.data[0].priceUsd);
            }
        }
    }
}