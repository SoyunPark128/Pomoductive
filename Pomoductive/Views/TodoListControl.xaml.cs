using Pomoductive.Models;
using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.UI.Popups;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The User Control item template is documented at https://go.microsoft.com/fwlink/?LinkId=234236

namespace Pomoductive.Views
{
    public sealed partial class TodoListControl : UserControl
    {
        public ApplicationViewModel ViewModel => App.AppViewModel;
        AppShell rootPage = AppShell.CurrentPage;

        public TodoListControl()
        {
            this.InitializeComponent();
        }
        

        private void TodoTreeView_Expanding(TreeView sender, TreeViewExpandingEventArgs args)
        {
            args.Node.HasUnrealizedChildren = false;
        }

        private void TodoTreeView_Collapsed(TreeView sender, TreeViewCollapsedEventArgs args)
        {
            args.Node.HasUnrealizedChildren = true;
        }

        public async void TodoTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var node = args.InvokedItem as TodoViewModel;

            if (ViewModel.SelectedTodo != node)
            {
                if (ViewModel.AppStopwatchViewModel.IsRunning)
                {
                    // Create the message dialog and set its content
                    var messageDialog = new MessageDialog("Do you Want to stop the current Pomodoro?");


                    // Add commands and set their command ids
                    messageDialog.Commands.Add(new UICommand("Stop", null, 0));
                    messageDialog.Commands.Add(new UICommand("Do not stop", null, 1));

                    // Set the command that will be invoked by default
                    messageDialog.DefaultCommandIndex = 1;

                    // Show the message dialog
                    var commandChosen = await messageDialog.ShowAsync();

                    if (commandChosen.Id is 0)
                    {
                        ViewModel.AppStopwatchViewModel.TimeCountStop();
                        ViewModel.SelectedTodo = node;
                        MainPage._pomodoroButton.FontSize = 55;
                        MainPage._pomodoroButton.Content = "Start";
                    }
                    else
                    {
                        //  Future Implement : Do not change selection highlight
                    }

                }
                else
                {
                    MainPage._pomodoroButton.FontSize = 55;
                    MainPage._pomodoroButton.Content = "Start";
                    ViewModel.SelectedTodo = node;
                }
            }
        }

        ///////////////////////////////////////
        ///

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

        /// <summary>
        /// Deletes the currently checked order.
        /// </summary>
        private async void Task_Finished_CheckAsync(object sender, RoutedEventArgs e)
        {
            var finishedTodoViewModel = (sender as FrameworkElement).DataContext as TodoViewModel;
            await finishedTodoViewModel.ReleaseTodo(true);
        }
        

        private async void TodoListTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            var currentTodoViewModel =  (sender as FrameworkElement).DataContext as TodoViewModel;
            var currentTextBox = sender as TextBox;
            currentTodoViewModel.Name = currentTextBox.Text;
            await currentTodoViewModel.SaveTodoAsync();
        }
    }
}
