using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;
using Windows.UI.Xaml;

namespace Pomoductive.ViewModels
{
    public class StopWatchViewModel
    {
        StopWatchModel StopWatchModel = StopWatchModel.Instance;
        //public event EventHandler<RoutedEventArgs> TimeCountStopEvent;

        public TimeSpan GetTotalTime()
        {
            return StopWatchModel.TotalTime;
        }

        public TimeSpan GetElapsedTime()
        {
            return StopWatchModel.Stopwatch.Elapsed;
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
                //TimeCountStopEvent.Invoke(this, null);
            }
            else
            {
                throw new Exception("not started");
            }
        }

        public bool IsRunning()
        {
            return StopWatchModel.Stopwatch.IsRunning;
        }

        private void TimeLog(TimeSpan elapsedTime)
        {
            StopWatchModel.TotalTime += elapsedTime;
        }

    }
}
