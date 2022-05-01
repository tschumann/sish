using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace sish
{
    class Sish
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Logger.Error("Please specify a three letter company code");

                System.Environment.Exit(1);
            }

            DateTime now = System.DateTime.Now;
            DateTime then = now.AddDays(-7);
            string code = args[0];
            string start = then.Year + "-" + then.Month + "-" + then.Day;
            string end = now.Year + "-" + now.Month + "-" + now.Day;
            DataLoader dataLoader = new DataLoader(code, start, end);

            // TODO: lots of dynamic below which makes this hard to figure debug
            dynamic prices = JsonConvert.DeserializeObject(dataLoader.prices);
            OrderedDictionary openPrices = new OrderedDictionary();

            List<dynamic> priceData = new List<object>();
            List<dynamic> openPriceData = new List<object>();

            for (int i = 0; i < prices.Elements.Count; i++)
            {
                if (prices.Elements[i].Type == "price")
                {
                    priceData = prices.Elements[i].ComponentSeries.ToObject<List<dynamic>>();
                }
            }

            for (int i = 0; i < priceData.Count; i++)
            {
                if (priceData[i].Type == "Open")
                {
                    openPriceData = priceData[i].Values.ToObject<List<dynamic>>();
                }
            }

            for (int i = 0; i < prices.Dates.Count; i++)
            {
                openPrices.Add(prices.Dates[i].ToString(), openPriceData[i].ToString());
            }
        }
    }
}
