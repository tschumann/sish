using System;
using System.IO;
using System.Net;
using System.Net.Http;

namespace sish
{
    class DataLoader
    {
        private static readonly HttpClient client = new HttpClient();
        private string data { get; }

        public DataLoader(string code)
        {
            string dataFileName = $"cache/{code}.json";

            try
            {
                data = File.ReadAllText(dataFileName);
                Logger.Info($"Found {dataFileName}");
            }
            catch (FileNotFoundException ex)
            {
                Logger.Info($"Couldn't find {dataFileName}");

                string statisticsUrl = $"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/key-statistics";

                HttpResponseMessage response = client.GetAsync(statisticsUrl).Result;

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    data = response.Content.ReadAsStringAsync().Result;

                    // TODO: create a logger class and log here
                    Console.WriteLine(data);

                    Directory.CreateDirectory("cache");

                    Logger.Info($"Writing data to {dataFileName}");
                    File.WriteAllText(dataFileName, data);
                }
                else
                {
                    Logger.Error($"Error from {statisticsUrl}: {response.ToString()}");
                }
            }            
        }
    }
}
