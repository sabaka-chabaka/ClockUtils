using System;
using ClockLib;

namespace ClockUtils
{
    internal class Program
    {
        static void Main()
        {
            Console.ForegroundColor = ConsoleColor.DarkGreen;
            Console.WriteLine("---WELCOME TO CLOCK UTILITIES---");
            Console.WriteLine("Commands: start, pause, resume, stop, history");

            var clock = new Utils();

            clock.Tick += time =>
            {
                Console.WriteLine(time);
            };

            while (true)
            {
                var command = Console.ReadLine();

                try
                {
                    switch (command)
                    {
                        case "start":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            clock.Start();
                            break;

                        case "pause":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            clock.Pause();
                            Console.WriteLine("Paused");
                            break;

                        case "resume":
                            Console.ForegroundColor = ConsoleColor.DarkRed;
                            clock.Resume();
                            Console.WriteLine("Resumed");
                            break;

                        case "stop":
                            Console.ForegroundColor = ConsoleColor.DarkGreen;
                            var result = clock.Stop();
                            Console.WriteLine("Final: " + Utils.Format(result));
                            break;

                        case "history":
                            Console.ForegroundColor = ConsoleColor.DarkMagenta;
                            if (clock.History.Count == 0)
                            {
                                Console.WriteLine("No records");
                                break;
                            }

                            for (int i = 0; i < clock.History.Count; i++)
                            {
                                Console.WriteLine(
                                    $"{i + 1}. {Utils.Format(clock.History[i])}");
                            }
                            break;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}
