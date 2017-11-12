using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler.Helpers
{
    public static class ConsoleClosure
    {
        public static ConsoleColor TextColor = ConsoleColor.Green;
        public static string HowToExitMsg = "Press CTRL+C to stop the application";

        public static string ExitAttentionMsg = "Are you sure you want to stop the application?";
        public static string ConfirmExitMsg = "Press ENTER to confirm or any button for cancel.";

        public static string CancelMsg = "Exit canceld. The application will continue to work.";

        public static void WaitForExit()
        {
            var reset = new ManualResetEvent(false);

            Console.CancelKeyPress += (s, e) => {

                e.Cancel = true;

                Write(ExitAttentionMsg);
                Write(ConfirmExitMsg);

                var key = ReadNewKey(true);

                if (key.Key == ConsoleKey.Enter)
                {
                    reset.Set();
                }
                else
                {
                    Write(CancelMsg);
                }
            };

            Write(HowToExitMsg);

            reset.WaitOne();
        }

        private static void Write(string msg)
        {
            Console.ForegroundColor = TextColor;
            Console.WriteLine();
            Console.WriteLine(msg);
            Console.WriteLine();
            Console.ResetColor();
        }

        public static ConsoleKeyInfo ReadNewKey(bool intercept = false)
        {
            while (Console.KeyAvailable)
                Console.ReadKey();

            return Console.ReadKey(intercept);
        }
    }
}
