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

            
            //Initiate StopWatch Event
            _pomodoreButton = PomodoreButtonText;

            if (StopWatch.IsRunning)
            {
                StopWatch.Interval = new TimeSpan(0, 0, 0);
                if (ViewModel.AppTimeRecordViewModel.Remainder != 0)
                {
                    StopWatch.RemainTime = Converters.RemainderToRemainTime(ViewModel.AppTimeRecordViewModel.Remainder, ViewModel.AppTimeRecordViewModel.TaskMin);
                }
                else
                {
                    StopWatch.RemainTime = TimeSpan.FromMinutes(selectedTodoViewModel.TaskMinutesPerOnePomo);
                }
            }
        }


        /// <summary>
        /// Gets the app-wide AppViewModel instance.
        /// </summary>
        ApplicationViewModel ViewModel => App.AppViewModel;
        StopWatchViewModel StopWatch => ViewModel.AppStopwatchViewModel;

        TodoViewModel selectedTodoViewModel = new TodoViewModel();
       
        public static Button _pomodoreButton = new Button();

        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
        {
            if (StopWatch.IsRunning)
            {
                StopWatch.TimeCountStop();
            }

            else
            {
                selectedTodoViewModel = ViewModel.SelectedTodo;
                ViewModel.AppTimeRecordViewModel = selectedTodoViewModel.GetTimeRecordViewModel();
                StopWatch.TimeCountStart();
                
            }
        }

    } 
}
