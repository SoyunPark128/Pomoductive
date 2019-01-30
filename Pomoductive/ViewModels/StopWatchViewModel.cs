using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;
using Pomoductive.Views;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Media;

namespace Pomoductive.ViewModels
{
    public class StopWatchViewModel : BindableBase
    {

        StopWatchModel StopWatchModel { get => StopWatchModel.Instance; }
        DispatcherTimer timer4Stopwatch = new DispatcherTimer();

        MediaPlayer player = new MediaPlayer();
        TimeSpan oneSecond = new TimeSpan(0, 0, 1);
        int shortBreakCount = 4;

        public StopWatchViewModel()
        {
            //Finish Sound
            player.Source = MediaSource.CreateFromUri(new Uri("ms-winsoundevent:Notification.Reminder"));

        }

        public TimeSpan Interval
        {
            get => timer4Stopwatch.Interval;
            set
            {
                if (value != timer4Stopwatch.Interval)
                {
                    timer4Stopwatch.Interval = value;
                    OnPropertyChanged();
                }
            }
        }

        private TimeSpan _remainTime = new TimeSpan();
        public TimeSpan RemainTime
        {
            get => _remainTime;
            set
            {
                if (value != _remainTime)
                {
                    _remainTime = value;
                    OnPropertyChanged();
                }
            }
        }

        private bool _isRunning;
        public bool IsRunning
        {
            get => _isRunning;
            set => Set(ref _isRunning, value);
        }

        private TimerStatus _currentStatus;
        public TimerStatus CurrentStatus
        {
            get => _currentStatus;
            set => Set(ref _currentStatus, value);
        }

        public TimeSpan GetTotalTime()
        {
            return StopWatchModel.TotalTime;
        }


        public TimeSpan ElapsedTime { get=> StopWatchModel.Stopwatch.Elapsed; }

        public void TimeCountStart()
        {
            timer4Stopwatch.Tick += TimerTicking;
            CurrentStatus = TimerStatus.InTodo;
            if (App.AppViewModel.AppTimeRecordViewModel.Remainder != 0)
            {
                RemainTime = Converters.RemainderToRemainTime(App.AppViewModel.AppTimeRecordViewModel.Remainder, App.AppViewModel.AppTimeRecordViewModel.TaskMin);
            }
            else
            {
                RemainTime = TimeSpan.FromMinutes(App.AppViewModel.SelectedTodo.TaskMinutesPerOnePomo);
            }
            MainPage._pomodoreButton.FontSize = 45;
            StopWatchModel.TotalTime = TimeSpan.Zero;
            StopWatchModel.Stopwatch.Start();
            timer4Stopwatch.Start();
            IsRunning = true;

        }
        
        public void TimeCountStop()
        {
            
            if (StopWatchModel.Stopwatch.IsRunning)
            {
                timer4Stopwatch.Stop();
                StopWatchModel.Stopwatch.Stop();
                TimeLog(StopWatchModel.Stopwatch.Elapsed);
                StopWatchModel.Stopwatch.Reset();
            }
            else
            {
                throw new Exception("not started");
            }
            IsRunning = false;
            timer4Stopwatch.Tick -= TimerTicking;
        }

       
        private void TimeLog(TimeSpan elapsedTime)
        {
            StopWatchModel.TotalTime += elapsedTime;
        }
        
        public async void TimerTicking(object sender, object e)
        {
            if (CurrentStatus == TimerStatus.InTodo)
            {
                await TodoMinutes();
            }

            else
            {
                RestMinutes();
            }
        }

        public async Task TodoMinutes()
        {
            MainPage._pomodoreButton.Background = new SolidColorBrush(Windows.UI.Colors.Tomato);
            if (RemainTime < TimeSpan.Zero)
            {
                //TimeCountStop();
                player.Play();
                //MainPage._pomodoreButton.FontSize = 55;
                //MainPage._pomodoreButton.Content = "Done!";

                App.AppViewModel.AppTimeRecordViewModel.Remainder = 0;
                App.AppViewModel.AppTimeRecordViewModel.TotalTaskCount++;
                await App.AppViewModel.AppTimeRecordViewModel.SaveTimeRecordAsync();
                App.AppViewModel.StatisticViewModels.TimeRecordViewModels.Add(App.AppViewModel.AppTimeRecordViewModel);

                // To start counting immediately
                timer4Stopwatch.Interval = new TimeSpan(0, 0, 0);
                

                // To Break
                if (shortBreakCount > 0)
                {
                    CurrentStatus = TimerStatus.ShortBreak;
                    RemainTime = TimeSpan.FromMinutes(App.AppViewModel.SelectedTodo.ShortBreakMinutesPerOnePomo);
                    shortBreakCount--;
                }
                else
                {
                    CurrentStatus = TimerStatus.LongBreak;
                    RemainTime = TimeSpan.FromMinutes(App.AppViewModel.SelectedTodo.LongBreakMinutesPerOnePomo);
                }

            }
            else
            {
                MainPage._pomodoreButton.Content = RemainTime.ToString(@"dd\:mm\:ss");
                RemainTime -= oneSecond;

                App.AppViewModel.AppTimeRecordViewModel.Remainder = Converters.RemainTimeToRemainder(RemainTime, App.AppViewModel.AppTimeRecordViewModel.TaskMin);
                
                await App.AppViewModel.AppTimeRecordViewModel.SaveTimeRecordAsync();
            }

            // For not too much pushing signals to DB
            timer4Stopwatch.Interval = oneSecond;
        }

        public void RestMinutes()
        {
            MainPage._pomodoreButton.Background = new SolidColorBrush(Windows.UI.Colors.GreenYellow);
            if (RemainTime < TimeSpan.Zero)
            {
                //TimeCountStop();
                player.Play();
                
                // To start counting immediately
                timer4Stopwatch.Interval = new TimeSpan(0, 0, 0);

                // To Break
                if (CurrentStatus == TimerStatus.LongBreak)
                {
                    shortBreakCount = 4;
                }
                CurrentStatus = TimerStatus.InTodo;
                RemainTime = TimeSpan.FromMinutes(App.AppViewModel.SelectedTodo.TaskMinutesPerOnePomo);
            }
            else
            {
                MainPage._pomodoreButton.Content = RemainTime.ToString(@"dd\:mm\:ss");
                RemainTime -= oneSecond;
            }

            timer4Stopwatch.Interval = oneSecond;
        }
    }

    public enum TimerStatus
    {
        InTodo = 0,
        ShortBreak = 1,
        LongBreak = 2
    }
}
