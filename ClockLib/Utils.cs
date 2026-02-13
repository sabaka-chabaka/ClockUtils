using System;
using System.Collections.Generic;
using System.Timers;

namespace ClockLib
{
    public class Utils : IDisposable
    {
        private DateTime _startTime;
        private DateTime _pauseStarted;
        private TimeSpan _pausedTime;

        private readonly Timer _timer;
        private readonly List<TimeSpan> _history = new();

        public bool IsRunning { get; private set; }
        public bool IsPaused { get; private set; }

        public IReadOnlyList<TimeSpan> History => _history.AsReadOnly();

        public event Action<string> Tick; // сразу готовая строка hh:mm:ss

        public Utils()
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTick;
            _timer.AutoReset = true;
        }

        public void Start()
        {
            if (IsRunning)
                throw new InvalidOperationException("Timer exists. First stop them");

            _startTime = DateTime.Now;
            _pausedTime = TimeSpan.Zero;

            IsRunning = true;
            IsPaused = false;

            _timer.Start();
        }

        public void Pause()
        {
            if (!IsRunning || IsPaused)
                return;

            _pauseStarted = DateTime.Now;
            IsPaused = true;
        }

        public void Resume()
        {
            if (!IsPaused)
                return;

            _pausedTime += DateTime.Now - _pauseStarted;
            IsPaused = false;
        }

        public TimeSpan Stop()
        {
            if (!IsRunning)
                throw new InvalidOperationException("Timer not running");

            var elapsed = DateTime.Now - _startTime - _pausedTime;

            _history.Add(elapsed);

            _timer.Stop();
            IsRunning = false;
            IsPaused = false;

            return elapsed;
        }

        private void OnTick(object sender, ElapsedEventArgs e)
        {
            if (IsRunning && !IsPaused)
            {
                Tick?.Invoke(Format(GetElapsed()));
            }
        }

        public TimeSpan GetElapsed()
        {
            if (!IsRunning)
                return TimeSpan.Zero;

            return DateTime.Now - _startTime - _pausedTime;
        }

        public static string Format(TimeSpan time)
        {
            return $"{(int)time.TotalHours:D2}:{time.Minutes:D2}:{time.Seconds:D2}";
        }

        public void Dispose()
        {
            _timer?.Dispose();
        }

        public DateTime CurrentDate()
        {
            return DateTime.Now;
        }

        public TimeZoneInfo CurrentTimeZoneInfo()
        {
            return TimeZoneInfo.Local;
        }
    }
}