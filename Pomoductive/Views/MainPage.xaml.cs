﻿using Pomoductive.Models;
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
        TimeRecordViewModel timeRecordViewModel;

        DispatcherTimer timer4Stopwatch = new DispatcherTimer();
        MediaPlayer player = new MediaPlayer();




        //public event EventHandler<RoutedEventArgs> PomodoreFinished;


        private async Task TodoCreateButtonAsync(object sender, RoutedEventArgs e)
        {
            Todo newTodo = new Todo(TodoNameInput.Text);
            TodoViewModel TodoViewModel = new TodoViewModel(newTodo)
            {
                IsNewTodo = true
            };

            TodoNameInput.ClearValue(TextBox.TextProperty);
            await TodoViewModel.SaveTodoAsync();

        }

        /// <summary>
        /// Add a sub To-do below current To-do
        /// </summary>
        private async void AddNewSubTodoAsync(object sender, RoutedEventArgs e)
        {
            var parentsTodoViewModel = (sender as FrameworkElement).DataContext as TodoViewModel;

            Todo newSubTodo = new Todo(AddNewSubTodoTextBox.Text, null, parentsTodoViewModel.Id);
            TodoViewModel newSubTodoViewModel = new TodoViewModel(newSubTodo)
            {
                IsNewTodo = true
            };

            AddNewSubTodoTextBox.ClearValue(TextBox.TextProperty);

            parentsTodoViewModel.IsNewTodo = false;
            await parentsTodoViewModel.SaveTodoAsync();
            await newSubTodoViewModel.SaveTodoAsync();
        }


        /// <summary>
        /// Deletes the currently checked order.
        /// </summary>
        private async void Task_Finished_CheckAsync(object sender, RoutedEventArgs e)
        {
            CheckBox checkedBox = (CheckBox)sender;

            var finishedTodoViewModel = (sender as FrameworkElement).DataContext as TodoViewModel;
            await finishedTodoViewModel.ReleaseTodo(true);
        }

        private void MoreButton_Click(object sender, RoutedEventArgs e)
        {
            Button MoreButton = (Button)sender;
            MoreButton.ContextFlyout.ShowAt(MoreButton);
            ViewModel.SelectedTodo = (e.OriginalSource as FrameworkElement).DataContext as TodoViewModel;
        }


        private async void DeleteTodoClickedAsync(object sender, RoutedEventArgs e)
        {
            AppBarButton appBarButton = (AppBarButton)sender;

            var deletedTodoViewModel = (sender as FrameworkElement).DataContext as TodoViewModel;
            await deletedTodoViewModel.DeleteTodo();
        }

        private void TimeCountingStartsButtonClicked(object sender, RoutedEventArgs e)
        {
            selectedTodoViewModel = App.AppViewModel.SelectedTodo;

            remainTime = TimeSpan.FromMinutes(selectedTodoViewModel.TaskMinutesPerOnePomo);
            timeRecordViewModel = selectedTodoViewModel.GetTimeRecordViewModel();

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
                PomodoreButtonText.Text = "Done!";

                timeRecordViewModel.Remainder = 0;
                timeRecordViewModel.TotalTaskCount++;
                await timeRecordViewModel.SaveTimeRecordAsync();
            }
            else
            {
                PomodoreButtonText.Text = remainTime.ToString(@"dd\:mm\:ss");
                remainTime = TimeSpan.FromMinutes(selectedTodoViewModel.TaskMinutesPerOnePomo) - ViewModel.Stopwatch.GetElapsedTime();

                timeRecordViewModel.Remainder = timeRecordViewModel.TaskMin / (float)ViewModel.Stopwatch.GetElapsedTime().Minutes;
                await timeRecordViewModel.SaveTimeRecordAsync();

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

        private void RenameTodoButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void RenameTodoAsync(object sender, RoutedEventArgs e)
        {

        }

        private void AddNewSubTodoFlyoutOpen(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                AddSubTodoButton.AllowFocusOnInteraction = true;
            }
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);

        }
        private void AddSubTodoFlyout_Closed(object sender, object e)
        {
            AddNewSubTodoTextBox.ClearValue(TextBox.TextProperty);
        }

    } 
}
