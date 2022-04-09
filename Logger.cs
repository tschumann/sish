using System;

namespace sish
{
    class Logger
    {
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
