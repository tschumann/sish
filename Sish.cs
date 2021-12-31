using System;
using System.IO;
using System.Net.Http;

namespace sish
{
    class Sish
    {
        private static readonly HttpClient client = new HttpClient();
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please specify a three letter company code");

                System.Environment.Exit(1);
            }

            string code = args[1];
            string statisticsUrl = $"https://asx.api.markitdigital.com/asx-research/1.0/companies/{code}/key-statistics";

            HttpResponseMessage response = client.GetAsync(statisticsUrl).Result;
            string result = response.Content.ReadAsStringAsync().Result;

            Console.WriteLine(result);

            Directory.CreateDirectory("cache");
            File.WriteAllText($"cache/{code}.json", result);
        }
    }
}
