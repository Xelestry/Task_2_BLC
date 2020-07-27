using System;
using Task_2_BLC.Interface;

namespace Task_2_BLC.Logs
{
    public class Logger : ILogger
    {
        public void Log(string message)
        {
            Console.WriteLine(message);
        }
    }
}
