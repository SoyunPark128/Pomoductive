using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Pomoductive.Model
{
    public class StopWatchModel
    {
        private Stopwatch _stopwatch;
        private TimeSpan _totalTime;

        public StopWatchModel()
        {
            _stopwatch = new Stopwatch();
            _totalTime = new TimeSpan();
        }

        public TimeSpan GetTotalTime()
        {
            return _totalTime;
        }

        public void TimeCountStart()
        {
            _totalTime = TimeSpan.Zero;
            _stopwatch.Start();
        }

        public void TimeCountReStart()
        {
            _stopwatch.Start();
        }

        public void TimeCountPause()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Start();
                TimeLog(_stopwatch.Elapsed);
            }
            else
            {
                throw new Exception("not started");
            }
        }
        public void TimeCountStop()
        {
            if (_stopwatch.IsRunning)
            {
                _stopwatch.Stop();
                TimeLog(_stopwatch.Elapsed);
                _stopwatch.Reset();
            }
            else
            {
                throw new Exception("not started");
            }
        }
        private void TimeLog(TimeSpan elapsedTime)
        {
            _totalTime += elapsedTime;
        }
    }
}
