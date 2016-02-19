using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DockerSignalsExample
{
    class Program
    {
        //http://stanislavs.org/stopping-command-line-applications-programatically-with-ctrl-c-events-from-net/

        static bool exitSystem = false;

        [DllImport("Kernel32.dll")]
        private static extern bool SetConsoleCtrlHandler(EventHandler handler, bool add);

        private delegate bool EventHandler(CtrlType sig);
        static EventHandler _handler;

        enum CtrlType
        {
            CTRL_C_EVENT = 0,
            CTRL_BREAK_EVENT = 1,
            CTRL_CLOSE_EVENT = 2,
            CTRL_LOGOFF_EVENT = 5,
            CTRL_SHUTDOWN_EVENT = 6
        }

        private static bool Handler(CtrlType sig)
        {
            Console.WriteLine("Exiting system due to external CTRL-C, or process kill, or shutdown");

            //do your cleanup here
            Thread.Sleep(2000); //simulate some cleanup delay

            Console.WriteLine("Cleanup complete");

            //allow main to run off
            shutdownEvent.Set();

            //shutdown right away so there are no lingering threads
            Environment.Exit(-1);

            return true;
        }

        /// <summary>
        /// waithandle for shutdown
        /// </summary>
        static ManualResetEvent shutdownEvent = new ManualResetEvent(false);

        static void Main(string[] args)
        {
            // Some biolerplate to react to close window event, CTRL-C, kill, etc
            _handler += new EventHandler(Handler);
            SetConsoleCtrlHandler(_handler, true);

            Console.WriteLine("Program start");

            //Console.CancelKeyPress += delegate
            //{
            //    Console.WriteLine("Shutdown order given.");
            //    // signal threads that wait for this
            //    shutdownEvent.Set();
            //};

            var sideThread = new Thread(LoopDateToConsole);
            sideThread.Start();

            // wait to get signaled
            // we do it this way because this is cross-thread
            shutdownEvent.WaitOne();
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
