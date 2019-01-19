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

        DispatcherTimer timer4Stopwatch = new DispatcherTimer();
        TimeSpan SettedTime = new TimeSpan(0, 0, 5);
        TimeSpan remainTime = new TimeSpan();
        TimeSpan padding = new TimeSpan(0, 0, 1);
        MediaPlayer player = new MediaPlayer();
        //public event EventHandler<RoutedEventArgs> PomodoreFinished;


        private async Task TodoCreateButtonAsync(object sender, RoutedEventArgs e)
        {
            Todo newTodo = new Todo(TodoNameInput.Text);
            TodoViewModel TodoViewModel = new TodoViewModel(newTodo)
            {
                Reward = "Sleep"
            };
            
            TodoNameInput.ClearValue(TextBox.TextProperty);
            await TodoViewModel.SaveAsync();
            
        }

        /// <summary>
        /// Add a sub To-do below current To-do
        /// </summary>
        private async void AddNewSubTodoAsync(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTodo != null)
            {
                Todo newSubTodo = new Todo(TodoNameInput.Text);
                TodoViewModel SubTodoViewModel = new TodoViewModel(newSubTodo);
            }
            

        }


        /// <summary>
        /// Deletes the currently checked order.
        /// </summary>
        private async void Task_Finished_Check(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTodo is null)
            {
                CheckBox checkedBox = (CheckBox)sender;
                ViewModel.SelectedTodo = (TodoViewModel)checkedBox.DataContext;
            }

            var deleteTodo = ViewModel.SelectedTodo;
            await ViewModel.DeleteTodo(deleteTodo);

        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            Button MoreButton = (Button)sender;
            MoreButton.ContextFlyout.ShowAt(MoreButton);
            ViewModel.SelectedTodo = (e.OriginalSource as FrameworkElement).DataContext as TodoViewModel;
        }

        private void OnClosed(object sender, object e)
        {

        }

        
        private async void DeleteTodoClickedAsync(object sender, RoutedEventArgs e)
        {

        }

        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
        {
            remainTime = SettedTime;
            
            ViewModel.Stopwatch.TimeCountStart();
            timer4Stopwatch.Start();
            
        }
        public void Timer_Tick4Stopwatch(object sender, object e)
        {

            if (remainTime < TimeSpan.Zero)
            {
                ViewModel.Stopwatch.TimeCountStop();
                timer4Stopwatch.Stop();
                player.Play();
                PomodoreButtonText.Text = "Done!";
                remainTimeTextBlock.Text = "Done!";
            }
            else
            {
                PomodoreButtonText.Text = remainTime.Add(padding).ToString(@"dd\:mm\:ss");
                remainTimeTextBlock.Text = remainTime.Add(padding).ToString(@"dd\:mm\:ss");
                remainTime = SettedTime - ViewModel.Stopwatch.GetElapsedTime();
            }
        }

        private void Button_PointerEntered(object sender, PointerRoutedEventArgs e)
        {
            if (ViewModel.Stopwatch.IsRunning())
            {
                PomodoreButtonText.Text = "Pause";
            }
            PomodoreButtonText.Text = "Restart";
        }

        private void Button_PointerExited(object sender, PointerRoutedEventArgs e)
        {
            PomodoreButtonText.Text = "Start";
        }


    }
}
