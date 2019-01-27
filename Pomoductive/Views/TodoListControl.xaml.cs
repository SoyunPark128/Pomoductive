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
        

        public TodoListControl()
        {
            this.InitializeComponent();
        }
        

        private void SampleTreeView_Expanding(TreeView sender, TreeViewExpandingEventArgs args)
        {
            args.Node.HasUnrealizedChildren = false;
        }

        private void SampleTreeView_Collapsed(TreeView sender, TreeViewCollapsedEventArgs args)
        {
            args.Node.HasUnrealizedChildren = true;
        }

        private void SampleTreeView_ItemInvoked(TreeView sender, TreeViewItemInvokedEventArgs args)
        {
            var node = args.InvokedItem as TodoViewModel;
            ViewModel.SelectedTodo = node;
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

    }
}
