using System;

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
            DataCache dataCache = new DataCache(code, start, end);
            DataParser dataParser = new DataParser(dataCache);

            Account account = new Account(0, 100);

            for (int i = 0; i < dataParser.openPrices.Count; i++)
            {
                Console.WriteLine(dataParser.openPrices[i]);
                if (i >= 7)
                {
                    float lastWeeksPrice = dataParser.openPrices[i - 7].Item2;
                    float todaysPrice = dataParser.openPrices[i].Item2;

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

            Console.WriteLine(account);
        }
    }
}
