using Newtonsoft.Json;
using System.Collections.Generic;
using System.Collections.Specialized;

namespace sish
{
    class DataParser
    {
        public OrderedDictionary openPrices = new OrderedDictionary();

        public DataParser(DataCache dataCache)
        {
            PriceData prices = JsonConvert.DeserializeObject<PriceData>(dataCache.prices);

            List<PriceDataComponentSeries> priceData = new List<PriceDataComponentSeries>();

            for (int i = 0; i < prices.elements.Count; i++)
            {
                if (prices.elements[i].type == "price")
                {
                    priceData = prices.elements[i].componentSeries;
                }
            }

            for (int i = 0; i < priceData.Count; i++)
            {
                if (priceData[i].type == "Open")
                {
                    for (int j = 0; j < prices.dates.Count; j++)
                    {
                        Logger.Info(prices.dates[j] + " " + priceData[i].values[j]);
                        openPrices.Add(prices.dates[j], priceData[i].values[j]);
                    }
                }
            }
        }
    }

    class PriceData
    {
        [JsonProperty("Dates")]
        public List<string> dates { get; set; }

        [JsonProperty("Elements")]
        public List<PriceDataElements> elements { get; set; }
    }

    class PriceDataElements
    {
        [JsonProperty("Type")]
        public string type { get; set; }

        [JsonProperty("ComponentSeries")]
        public List<PriceDataComponentSeries> componentSeries { get; set; }
    }

    class PriceDataComponentSeries
    {
        [JsonProperty("Type")]
        public string type { get; set; }

        [JsonProperty("Values")]
        public List<float> values { get; set; }
    }
}
