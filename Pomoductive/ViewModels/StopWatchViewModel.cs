using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;

namespace Pomoductive.ViewModels
{
    public class StopWatchViewModel
    {
        StopWatchModel StopWatchModel = StopWatchModel.Instance;

        public TimeSpan GetTotalTime()
        {
            return StopWatchModel.TotalTime;
        }

        public void TimeCountStart()
        {
            StopWatchModel.TotalTime = TimeSpan.Zero;
            StopWatchModel.Stopwatch.Start();
        }

        public void TimeCountReStart()
        {
            StopWatchModel.Stopwatch.Start();
        }

        public void TimeCountStop()
        {
            if (StopWatchModel.Stopwatch.IsRunning)
            {
                StopWatchModel.Stopwatch.Stop();
                TimeLog(StopWatchModel.Stopwatch.Elapsed);
                StopWatchModel.Stopwatch.Reset();
            }
            else
            {
                throw new Exception("not started");
            }
        }
        private void TimeLog(TimeSpan elapsedTime)
        {
            StopWatchModel.TotalTime += elapsedTime;
        }
    }
}
