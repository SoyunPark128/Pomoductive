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

namespace Pomoductive
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class MainPage : Page
    {
        public MainPage()
        {
            this.InitializeComponent();
            player.Source = MediaSource.CreateFromUri(new Uri("ms-winsoundevent:Notification.Reminder"));
        }


        /// <summary>
        /// Gets the app-wide AppViewModel instance.
        /// </summary>
        public ApplicationViewModel ViewModel => App.AppViewModel;

        DispatcherTimer timer4Stopwatch = new DispatcherTimer();
        TimeSpan SettedTime = new TimeSpan(0, 0, 5);
        TimeSpan remainTime = new TimeSpan();
        TimeSpan padding = new TimeSpan(0, 0, 1); 
        MediaPlayer player = new MediaPlayer();
        //public event EventHandler<RoutedEventArgs> PomodoreFinished;


        private async Task Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            Todo newTodo = new Todo(TaskNameInput.Text);
            TodoViewModel TodoViewModel = new TodoViewModel(newTodo)
            {
                Reward = "Sleep"
            };

            CheckBox taskCheckBox = new CheckBox();
            taskCheckBox.Name = "Task" + newTodo.Name;
            taskCheckBox.Content = newTodo.Name;
            taskCheckBox.Checked += Task_Finished_Check;
            

            TaskListPanel.Children.Add(taskCheckBox);
            TaskNameInput.ClearValue(TextBox.TextProperty);
            
            await TodoViewModel.SaveAsync();
            

        }

        private void Task_Finished_Check(object sender, RoutedEventArgs e)
        {

        }


        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
    {
            
            Button clickedButton = (Button)sender;
            remainTime = SettedTime;

            EventHandler<object> tmr4SwTickEventHndlr = null;
            tmr4SwTickEventHndlr = (object s, object a) =>
            {
                Timer_Tick4Stopwatch(s, a, clickedButton, ref tmr4SwTickEventHndlr);
            };
            timer4Stopwatch.Tick += tmr4SwTickEventHndlr;

            ViewModel.Stopwatch.TimeCountStart();
            timer4Stopwatch.Start();
            
            testSenderText.Text = "object sender : " + sender?.ToString() ?? "object sender is Nothing";
            testEText.Text = "RoutedEventArgs e.OriginalSource : " + e.OriginalSource?.ToString() ?? "RoutedEventArgs e.OriginalSource is Nothing";
        }
        public void Timer_Tick4Stopwatch(object sender, object e, Button clickedButton, ref EventHandler<object> TickEventHandlr)
        {
            
            if (remainTime < TimeSpan.Zero)
            {
                ViewModel.Stopwatch.TimeCountStop();
                timer4Stopwatch.Stop();
                timer4Stopwatch.Tick -= TickEventHandlr;
                player.Play();
                clickedButton.Content = "Done!";
                remainTimeTextBlock.Text = "Done!";
            }
            else
            {
                clickedButton.Content = remainTime.Add(padding).ToString(@"dd\:mm\:ss");
                remainTimeTextBlock.Text = remainTime.Add(padding).ToString(@"dd\:mm\:ss");
                remainTime = SettedTime - ViewModel.Stopwatch.GetElapsedTime();
            }
        }
    }
}
