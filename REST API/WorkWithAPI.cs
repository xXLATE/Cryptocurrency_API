using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Telegram.Bot;

namespace REST_API
{
    class WorkWithAPI
    {
        public static void Main()
        {
            TelegramBotClient bot = new TelegramBotClient("1730877816:AAEthbQ8wi87EC721d76lLyin3vtjqVNuUs");

            bot.OnMessage += (s, arg) =>
            {
                string currency = arg.Message.Text;
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
                    Console.WriteLine($"{arg.Message.Chat.FirstName}: {arg.Message.Text}");
                    bot.SendTextMessageAsync(arg.Message.Chat.Id, coinsForecast.data[0].priceUsd);
                }
            };
            bot.StartReceiving();

            Console.ReadKey();
        }
    }
}