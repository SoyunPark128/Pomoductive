using Pomoductive.Models;
using Pomoductive.ViewModels;
using System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;

namespace Pomoductive.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();

            
            //Initiate StopWatch Event
            _pomodoroButton = PomodoroButtonText;

            if (StopWatch.IsRunning)
            {
                StopWatch.Interval = new TimeSpan(0, 0, 0);
                if (ViewModel.AppTimeRecordViewModel.Remainder != 0 && StopWatch.CurrentStatus == TimerStatus.InTodo)
                {
                    StopWatch.RemainTime = Converters.RemainderToRemainTime(ViewModel.AppTimeRecordViewModel.Remainder, ViewModel.AppTimeRecordViewModel.TaskMin);
                }
                else if (ViewModel.AppTimeRecordViewModel.Remainder == 0 && StopWatch.CurrentStatus == TimerStatus.InTodo)
                {
                    StopWatch.RemainTime = TimeSpan.FromMinutes(ViewModel.SelectedTodo.TaskMinutesPerOnePomo);
                }
            }
        }


        /// <summary>
        /// Gets the app-wide AppViewModel instance.
        /// </summary>
        ApplicationViewModel ViewModel => App.AppViewModel;
        StopWatchViewModel StopWatch => ViewModel.AppStopwatchViewModel;
        StatisticDataViewModel StatisticViewModel => App.AppStatisticDataViewModel;
        

        public static Button _pomodoroButton = new Button();

        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (StopWatch.IsRunning)
            {
                StopWatch.TimeCountStop();
            }

            else
            {
                ViewModel.AppTimeRecordViewModel = ViewModel.SelectedTodo.GetTimeRecordViewModel();
                StopWatch.TimeCountStart();
                
            }
        }

    } 
}
