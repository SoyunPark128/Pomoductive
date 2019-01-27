using Pomoductive.Models;
using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Core;
using Windows.Media.Playback;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=402352&clcid=0x409

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

            //Finish Sound
            player.Source = MediaSource.CreateFromUri(new Uri("ms-winsoundevent:Notification.Reminder"));

            //Initiate StopWatch Event
            timer4Stopwatch.Tick += Timer_Tick4Stopwatch;
            
        }


        /// <summary>
        /// Gets the app-wide AppViewModel instance.
        /// </summary>
        public ApplicationViewModel ViewModel => App.AppViewModel;
        TodoViewModel selectedTodoViewModel = new TodoViewModel();
        TimeSpan remainTime = new TimeSpan();
        TimeSpan OneSecond = new TimeSpan(0, 0, 1);
        TimeRecordViewModel timeRecordViewModel;

        DispatcherTimer timer4Stopwatch = new DispatcherTimer();
        
        MediaPlayer player = new MediaPlayer();


        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (ViewModel.Stopwatch.IsRunning)
            {

            }
            


            selectedTodoViewModel = App.AppViewModel.SelectedTodo;
            timeRecordViewModel = selectedTodoViewModel.GetTimeRecordViewModel();

            if (timeRecordViewModel.Remainder != 0)
            {
                remainTime = Converters.RemainderToRemainTime(timeRecordViewModel.Remainder, timeRecordViewModel.TaskMin);
            }
            else
            {
                remainTime = TimeSpan.FromMinutes(selectedTodoViewModel.TaskMinutesPerOnePomo);
            }

            ViewModel.Stopwatch.TimeCountStart();
            PomodoreButtonText.FontSize = 45;
            timer4Stopwatch.Start();
        }

        public async void Timer_Tick4Stopwatch(object sender, object e)
        {
            if (remainTime < TimeSpan.Zero)
            {
                ViewModel.Stopwatch.TimeCountStop();
                timer4Stopwatch.Stop();
                player.Play();
                PomodoreButtonText.FontSize = 55;
                PomodoreButtonText.Content = "Done!";

                timeRecordViewModel.Remainder = 0;
                timeRecordViewModel.TotalTaskCount++;
                await timeRecordViewModel.SaveTimeRecordAsync();

                // To start counting immediately
                timer4Stopwatch.Interval = new TimeSpan(0, 0, 0);
                remainTime = TimeSpan.FromMinutes(selectedTodoViewModel.TaskMinutesPerOnePomo);

            }
            else
            {
                PomodoreButtonText.Content = remainTime.ToString(@"dd\:mm\:ss");
                remainTime -= OneSecond;
                
                timeRecordViewModel.Remainder = Converters.RemainTimeToRemainder(remainTime, timeRecordViewModel.TaskMin);

                {
                    ElapsedMinTxt.Text = "timeRecordViewModel.Remainder : " + timeRecordViewModel.Remainder.ToString();
                    min.Text = "remainTime.Minutes : " + remainTime.Minutes.ToString();
                    totalmin.Text = "remainTime.TotalMinutes : " + remainTime.TotalMinutes.ToString();
                    stopwatchElapsed.Text = "ViewModel.Stopwatch.ElapsedTime : " + ViewModel.Stopwatch.ElapsedTime.ToString();
                }
                await timeRecordViewModel.SaveTimeRecordAsync();
            }

            // For not too much pushing signals to DB
            timer4Stopwatch.Interval = new TimeSpan(0, 0, 1);

        }
    } 
}
