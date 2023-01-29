using System;
using System.Threading;
using LogTest;

namespace LogUsers
{


    class Program
    {
        private const string _logLocation = @"C:\LogTest";

        static void Main(string[] args)
        {

            ILogWriter logWriter = new FileLogWriter(_logLocation);

            ILog  logger = new AsyncLog(logWriter);

            for (int i = 0; i < 15; i++)
            {
                logger.Write("Number with Flush: " + i.ToString());
                Thread.Sleep(200);
            }
     

            logger.StopWithFlush();

            logWriter = new FileLogWriter(_logLocation);
            ILog logger2 = new AsyncLog(logWriter);

            for (int i = 50; i > 0; i--)
            {
                logger2.Write("Number with No flush: " + i.ToString());
                Thread.Sleep(40);
            }

            logger2.StopWithoutFlush();

            Console.ReadLine();
        }
    }
}
