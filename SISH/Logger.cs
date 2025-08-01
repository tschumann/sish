using System;

namespace sish
{
    public class Logger
    {
        public static void Trace(string message)
        {
            Console.WriteLine(message);
        }

        public static void Info(string message)
        {
            Console.WriteLine(message);
        }

        public static void Error(string message)
        {
            Console.Error.WriteLine(message);
        }
    }
}
