using System;
using System.IO;
using System.Net;
using Newtonsoft.Json;
using Telegram.Bot;

namespace REST_API
{
    class WorkWithAPI
    {
        [Obsolete]
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

                    try
                    {
                        bot.SendTextMessageAsync(arg.Message.Chat.Id,
                            $"Name: {coinsForecast.data[0].name}\n" +
                            $"USD price: {coinsForecast.data[0].priceUsd}$\n" +
                            $"Position in top: {coinsForecast.data[0].rank}\n" +
                            $"Capitalization: {coinsForecast.data[0].marketCapUsd}$\n" +
                            $"Volume in 24 hours: {coinsForecast.data[0].volumeUsd24Hr}$\n" +
                            $"Changes in 24 hours: {coinsForecast.data[0].changePercent24Hr}%\n");

                        Console.WriteLine($"{arg.Message.Chat.FirstName}: {arg.Message.Text}");
                    }

                    catch (Exception)
                    {
                        bot.SendTextMessageAsync(arg.Message.Chat.Id, "Error: unknown cryptocurrency");
                        Console.WriteLine($"{arg.Message.Chat.FirstName}: {arg.Message.Text}");
                    }
                }
            };
            bot.StartReceiving();

            Console.ReadKey();
        }
    }
}