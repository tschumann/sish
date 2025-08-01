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

                Environment.Exit(1);
            }

            int days = 90;
            Simulator simulator = new Simulator();
            simulator.account.setStartingBalance(100.0f);

            for (int i = 1; i < args.Length; i++)
            {
                if (args[i] == "--buy-fee-percent")
                {
                    simulator.account.buyFeePercent = Int32.Parse(args[i + 1]);
                    i++;
                }
                if (args[i] == "--sell-fee-percent")
                {
                    simulator.account.sellFeePercent = Int32.Parse(args[i + 1]);
                    i++;
                }
                if (args[i] == "--days")
                {
                    days = Int32.Parse(args[i + 1]);
                    i++;
                }
            }

            DateTime now = DateTime.Now;
            DateTime then = now.AddDays(-days);
            string code = args[0];
            string start = then.Year + "-" + then.Month + "-" + then.Day;
            string end = now.Year + "-" + now.Month + "-" + now.Day;
            DataCache dataCache = new DataCache(code, start, end, days);
            DataParser dataParser = new DataParser(dataCache);

            Logger.Info("Start: " + simulator.account);

            simulator.Run(code, dataParser.openPrices);

            Logger.Info("End: " + simulator.account);
        }
    }
}
