using System;
using System.IO;
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
            }
            catch(Exception ex)
            {
                string statisticsUrl = $"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/key-statistics";

                HttpResponseMessage response = client.GetAsync(statisticsUrl).Result;
                data = response.Content.ReadAsStringAsync().Result;

                // TODO: create a logger class and log here
                Console.WriteLine(data);

                Directory.CreateDirectory("cache");
                File.WriteAllText(dataFileName, data);
            }            
        }
    }
}
