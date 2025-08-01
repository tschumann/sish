using System;
using System.Collections.Generic;

namespace sish
{
    public class Simulator
    {
        public Account account { get; }

        public Simulator()
        {
            account = new Account();
        }

        public void Run(string code, List<(DateTime, float)> prices)
        {
            for (int i = 0; i < prices.Count; i++)
            {
                if (i >= account.timePeriod)
                {
                    float previousPrice = prices[i - account.timePeriod].Item2;
                    float currentPrice = prices[i].Item2;

                    if (account.CanSell(1) && ShouldSell(currentPrice, previousPrice))
                    {
                        Logger.Trace("Selling (price: " + prices[i] + ")");
                        account.Sell(code, 1, currentPrice, DateTime.Now);
                    }
                    else if (account.CanBuy(1, currentPrice) && ShouldBuy(currentPrice, previousPrice))
                    {
                        Logger.Trace("Buying (price: " + prices[i] + ")");
                        account.Buy(code, 1, currentPrice, DateTime.Now);
                    }
                }
            }
        }

        public bool ShouldBuy(float currentPrice, float previousPrice)
        {
            return currentPrice < previousPrice;
        }

        public bool ShouldSell(float currentPrice, float previousPrice)
        {
            return currentPrice > previousPrice;
        }
    }
}
