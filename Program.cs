using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ClockUtils
{
    internal class Program
    {
        private static DateTime _startTime;
        private static TimeSpan _pausedTime = TimeSpan.Zero;
        private static DateTime _pauseStarted;

        private static bool _isRunning;
        private static bool _isPaused;

        private static readonly List<TimeSpan> _history = new();

        static async Task Main()
        {
            Console.WriteLine("--- WELCOME TO CLOCK UTILITIES ---");
            Console.WriteLine("Commands: start, pause, resume, stop, history, exit");

            _ = Task.Run(SecondsMeter);

            while (true)
            {
                var command = Console.ReadLine();

                switch (command)
                {
                    case "start":
                        if (_isRunning)
                        {
                            Console.WriteLine("Timer already running");
                            break;
                        }

                        _startTime = DateTime.Now;
                        _pausedTime = TimeSpan.Zero;
                        _isRunning = true;
                        _isPaused = false;

                        Console.WriteLine("Timer started");
                        break;

                    case "pause":
                        if (!_isRunning || _isPaused)
                        {
                            Console.WriteLine("Nothing to pause");
                            break;
                        }

                        _pauseStarted = DateTime.Now;
                        _isPaused = true;
                        Console.WriteLine("Paused");
                        break;

                    case "resume":
                        if (!_isPaused)
                        {
                            Console.WriteLine("Timer is not paused");
                            break;
                        }

                        _pausedTime += DateTime.Now - _pauseStarted;
                        _isPaused = false;
                        Console.WriteLine("Resumed");
                        break;

                    case "stop":
                        if (!_isRunning)
                        {
                            Console.WriteLine("Timer not running");
                            break;
                        }

                        var elapsed = DateTime.Now - _startTime - _pausedTime;
                        _history.Add(elapsed);

                        Console.WriteLine($"Stopped at {Format(elapsed)}");

                        _isRunning = false;
                        _isPaused = false;
                        break;

                    case "history":
                        if (_history.Count == 0)
                        {
                            Console.WriteLine("No records yet");
                            break;
                        }

                        for (int i = 0; i < _history.Count; i++)
                        {
                            Console.WriteLine($"{i + 1}. {Format(_history[i])}");
                        }
                        break;

                    case "exit":
                        return;
                }
            }
        }

        private static async Task SecondsMeter()
        {
            while (true)
            {
                if (_isRunning && !_isPaused)
                {
                    var elapsed = DateTime.Now - _startTime - _pausedTime;
                    Console.WriteLine(Format(elapsed));
                }

                await Task.Delay(1000);
            }
        }

        private static string Format(TimeSpan time)
        {
            return $"{time.Hours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }
    }
}