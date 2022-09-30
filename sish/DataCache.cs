using Newtonsoft.Json;
using System.IO;
using System.Net.Http;
using System.Net.Http.Headers;

namespace sish
{
    /// <summary>
    /// Class <c>DataCache</c> abstracts away the data storage. It gets it and caches it if required, otherwise it pulls from the cache.
    /// </summary>
    public class DataCache
    {
        private static readonly HttpClient client = new HttpClient();
        private string statistics { get; }
        private string header { get; }
        public string prices { get; }

        /// <summary>
        /// Download the price data for the given stock code between the given start and end times which should be a time period equalling the given number of days.
        /// </summary>
        public DataCache(string code, string start, string end, int days)
        {
            string statisticsFilename = $"cache/{code}-statistics.json";
            string headerFilename = $"cache/{code}-header.json";
            string priceDataFilename = $"cache/{code}-prices_{start}_{end}.json";

            statistics = getOrLoadJsonFile($"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/key-statistics", statisticsFilename);
            header = getOrLoadJsonFile($"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/header", headerFilename);

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
            // TODO: work out a neat way to substitute stuff in here
            request.Content = new StringContent("{\"days\":" + days + ",\"dataNormalized\":false,\"dataPeriod\":\"Day\",\"dataInterval\":1,\"realtime\":false,\"yFormat\":\"0.###\",\"timeServiceFormat\":\"JSON\",\"rulerIntradayStart\":26,\"rulerIntradayStop\":3,\"rulerInterdayStart\":10957,\"rulerInterdayStop\":365,\"returnDateType\":\"ISO8601\",\"elements\":[{\"Label\":\"0c919013\",\"Type\":\"price\",\"Symbol\":\"" + symbol + "\",\"OverlayIndicators\":[],\"Params\":{}},{\"Label\":\"4c2fff58\",\"Type\":\"volume\",\"Symbol\":\"" + symbol + "\",\"OverlayIndicators\":[],\"Params\":{}}]}");
            request.Content.Headers.ContentType = MediaTypeHeaderValue.Parse("application/x-www-form-urlencoded");

            try
            {
                prices = File.ReadAllText(priceDataFilename);
                Logger.Info($"Found {priceDataFilename}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {priceDataFilename}");

                HttpResponseMessage response = client.SendAsync(request).Result;

                if (response.IsSuccessStatusCode)
                {
                    prices = response.Content.ReadAsStringAsync().Result;

                    Logger.Trace(prices);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {priceDataFilename}");
                    File.WriteAllText(priceDataFilename, prices);
                }
                else
                {
                    Logger.Error($"Error from {pricesUrl}: {response.ToString()}");
                }
            }
        }

        /// <summary>
        /// Return the JSON data from the local cache, or if it isn't cached, get it from the given URL and cache it.
        /// </summary>
        public static string getOrLoadJsonFile(string url, string fileName)
        {
            string json = null;

            try
            {
                json = File.ReadAllText(fileName);
                Logger.Info($"Found {fileName}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {fileName}");

                HttpResponseMessage response = client.GetAsync(url).Result;

                if (response.IsSuccessStatusCode)
                {
                    json = response.Content.ReadAsStringAsync().Result;

                    Logger.Trace(json);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {fileName}");
                    File.WriteAllText(fileName, json);
                }
                else
                {
                    Logger.Error($"Error from {url}: {response.ToString()}");
                }
            }

            return json;
        }
    }
}
