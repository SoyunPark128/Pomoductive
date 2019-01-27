using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;
using Windows.UI.Xaml;

namespace Pomoductive.ViewModels
{
    public class StopWatchViewModel  : BindableBase
    {
        public TimeSpan remainTime = new TimeSpan();
        //public event EventHandler<RoutedEventArgs> TimeCountStopEvent;
        
        public StopWatchModel StopWatchModel{ get => StopWatchModel.Instance; }

        public TimeSpan GetTotalTime()
        {
            return StopWatchModel.TotalTime;
        }


        public TimeSpan ElapsedTime { get=> StopWatchModel.Stopwatch.Elapsed; }

        public void TimeCountStart()
        {
            StopWatchModel.TotalTime = TimeSpan.Zero;
            StopWatchModel.Stopwatch.Start();
            IsRunning = true;
        }

        public void TimeCountReStart()
        {
            StopWatchModel.Stopwatch.Start();
            IsRunning = true;
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
            IsRunning = false;
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => Set(ref _isRunning, value);
        }

        private void TimeLog(TimeSpan elapsedTime)
        {
            StopWatchModel.TotalTime += elapsedTime;
        }
        
    }
}
