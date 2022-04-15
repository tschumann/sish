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

            string code = args[0];
            DataLoader dataLoader = new DataLoader(code);
        }
    }
}
