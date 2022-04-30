using System;

namespace sish
{
    class Sish
    {
        static void Main(string[] args)
        {
            Console.WriteLine(args[0]);
            if (args.Length < 1)
            {
                Console.WriteLine("Please specify a three letter company code");

                System.Environment.Exit(1);
            }

            DateTime now = System.DateTime.Now;
            DateTime then = now.AddDays(-7);
            string code = args[0];
            string start = then.Year + "-" + then.Month + "-" + then.Day;
            string end = now.Year + "-" + now.Month + "-" + now.Day;
            DataLoader dataLoader = new DataLoader(code, start, end);
        }
    }
}
