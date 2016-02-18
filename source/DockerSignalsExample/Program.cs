using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerSignalsExample
{
    class Program
    {
        /// <summary>
        /// waithandle for shutdown
        /// </summary>
        static ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            Console.WriteLine("Program start");

            Console.CancelKeyPress += Console_CancelKeyPress;

            var sideThread = new Thread(LoopDateToConsole);
            sideThread.Start();

            // wait to get signaled
            // we do it this way because this is cross-thread
            shutdownEvent.WaitOne();
        }

        private static void Console_CancelKeyPress(object sender, ConsoleCancelEventArgs e)
        {
            Console.WriteLine("Shutdown order given.");
            // signal threads that wait for this
            shutdownEvent.Set();
        }

        private static void LoopDateToConsole()
        {
            while (true)
            {
                Console.WriteLine(DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
                Thread.Sleep(2000);
            }
        }
    }
}
