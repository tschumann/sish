using System;
using System.Collections.Generic;

namespace sish
{
    public class Simulator
    {
        public Account account { get; }

        public Simulator()
        {
            account = new Account(0, 100);
        }

        public void Run(string code, List<(string, float)> prices)
        {
            for (int i = 0; i < prices.Count; i++)
            {
                Console.WriteLine(prices[i]);
                if (i >= 7)
                {
                    float lastWeeksPrice = prices[i - 7].Item2;
                    float todaysPrice = prices[i].Item2;

                    if (todaysPrice > lastWeeksPrice && account.CanSell(1))
                    {
                        account.Sell(code, 1, todaysPrice);
                    }
                    else if (todaysPrice > lastWeeksPrice && account.CanBuy(1, todaysPrice))
                    {
                        account.Buy(code, 1, todaysPrice);
                    }
                }
            }
        }
    }
}
