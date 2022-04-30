using Newtonsoft.Json;
using System;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace sish
{
    public class DataLoader
    {
        private static readonly HttpClient client = new HttpClient();
        private string statistics { get; }
        private string header { get; }
        private string opens { get; }

        public DataLoader(string code, string start, string end)
        {
            string statisticsFilename = $"cache/{code}-statistics.json";
            string headerFilename = $"cache/{code}-header.json";
            string openPriceDataFilename = $"cache/{code}-open-prices_{start}_{end}.json";

            try
            {
                statistics = File.ReadAllText(statisticsFilename);
                Logger.Info($"Found {statisticsFilename}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {statisticsFilename}");

                string statisticsUrl = $"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/key-statistics";

                HttpResponseMessage statisticsResponse = client.GetAsync(statisticsUrl).Result;

                if (statisticsResponse.IsSuccessStatusCode)
                {
                    statistics = statisticsResponse.Content.ReadAsStringAsync().Result;

                    // TODO: create a logger class and log here
                    Console.WriteLine(statistics);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {statisticsFilename}");
                    File.WriteAllText(statisticsFilename, statistics);
                }
                else
                {
                    Logger.Error($"Error from {statisticsUrl}: {statisticsResponse.ToString()}");
                }
            }

            try
            {
                header = File.ReadAllText(headerFilename);
                Logger.Info($"Found {headerFilename}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {headerFilename}");

                string headerUrl = $"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/header";

                HttpResponseMessage headerResponse = client.GetAsync(headerUrl).Result;

                if (headerResponse.IsSuccessStatusCode)
                {
                    header = headerResponse.Content.ReadAsStringAsync().Result;

                    // TODO: create a logger class and log here
                    Console.WriteLine(header);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {headerFilename}");
                    File.WriteAllText(headerFilename, statistics);
                }
                else
                {
                    Logger.Error($"Error from {headerUrl}: {headerResponse.ToString()}");
                }
            }

            dynamic headerData = JsonConvert.DeserializeObject(header);
            string pricesUrl = "https://api.markitondemand.com/apiman-gateway/MOD/chartworks-data/1.0/chartapi/series?access_token=83ff96335c2d45a094df02a206a39ff4";
            HttpRequestMessage request = new HttpRequestMessage(new HttpMethod("POST"), pricesUrl);
            request.Headers.TryAddWithoutValidation("Accept", "*/*");
            request.Headers.TryAddWithoutValidation("Accept-Language", "en-US,en;q=0.9");
            request.Headers.TryAddWithoutValidation("Connection", "keep-alive");
            request.Headers.TryAddWithoutValidation("Origin", "https://www2.asx.com.au");
            request.Headers.TryAddWithoutValidation("Referer", "https://www2.asx.com.au/");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Dest", "empty");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Mode", "cors");
            request.Headers.TryAddWithoutValidation("Sec-Fetch-Site", "cross-site");
            request.Headers.TryAddWithoutValidation("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/100.0.4896.127 Safari/537.36");
            request.Headers.TryAddWithoutValidation("sec-ch-ua", "\" Not A;Brand\";v=\"99\", \"Chromium\";v=\"100\", \"Google Chrome\";v=\"100\"");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-mobile", "?0");
            request.Headers.TryAddWithoutValidation("sec-ch-ua-platform", "\"Windows\"");

            string symbol = headerData.data.xid;
            request.Content = new StringContent("{\"days\":91,\"dataNormalized\":false,\"dataPeriod\":\"Day\",\"dataInterval\":1,\"realtime\":false,\"yFormat\":\"0.###\",\"timeServiceFormat\":\"JSON\",\"rulerIntradayStart\":26,\"rulerIntradayStop\":3,\"rulerInterdayStart\":10957,\"rulerInterdayStop\":365,\"returnDateType\":\"ISO8601\",\"elements\":[{\"Label\":\"0c919013\",\"Type\":\"price\",\"Symbol\":\"" + symbol + "\",\"OverlayIndicators\":[],\"Params\":{}},{\"Label\":\"4c2fff58\",\"Type\":\"volume\",\"Symbol\":\"" + symbol + "\",\"OverlayIndicators\":[],\"Params\":{}}]}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            try
            {
                opens = File.ReadAllText(openPriceDataFilename);
                Logger.Info($"Found {openPriceDataFilename}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {openPriceDataFilename}");

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    statistics = response.Content.ReadAsStringAsync().Result;

                    // TODO: create a logger class and log here
                    Console.WriteLine(statistics);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {openPriceDataFilename}");
                    File.WriteAllText(openPriceDataFilename, statistics);
                }
                else
                {
                    Logger.Error($"Error from {pricesUrl}: {response.ToString()}");
                }
            }
        }
    }
}
