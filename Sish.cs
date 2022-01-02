using System;

namespace sish
{
    class Sish
    {
        static void Main(string[] args)
        {
            if (args.Length < 2)
            {
                Console.WriteLine("Please specify a three letter company code");

                System.Environment.Exit(1);
            }

            string code = args[1];
            DataLoader dataLoader = new DataLoader(code);
        }
    }
}
