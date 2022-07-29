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

            Simulator simulator = new Simulator();

            Logger.Info("Start: " + simulator.account);

            simulator.Run(code, dataParser.openPrices);

            Logger.Info("End: " + simulator.account);
        }
    }
}
