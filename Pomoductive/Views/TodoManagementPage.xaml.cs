using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Storage;
using Windows.System;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pomoductive.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoManagementPage : Page
    {
        public TodoListViewModel ViewModel { get; } = new TodoListViewModel();

        public TodoManagementPage()
        {
            this.InitializeComponent();
        }

        private async void NonTextControl_Changed(object sender, RoutedEventArgs e)
        {
            await ViewModel.SelectedTodo.SaveTodoAsync();
        }

        private async void IsTerminatedToggleButton_Toggled(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTodo is null || ViewModel.IsInEdit == false)
            {
                return;
            }
            ViewModel.SelectedTodo.IsTerminated = !ViewModel.SelectedTodo.IsTerminated;
            await ViewModel.SelectedTodo.SaveTodoAsync();
        }

        private void Listview_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ViewModel.IsInEdit = false;

            IsTerminatedToggleButton.IsOn = !ViewModel.SelectedTodo.IsTerminated;
            ViewModel.GetParentsTodoList();

            TodoViewModel _selectedItem =  ViewModel.ParrentsTodos.FirstOrDefault(t => t.Id == ViewModel.SelectedTodo.ParentsTodoId);
            if (_selectedItem is null)
            {
                ParentsComboBox.SelectedIndex = 0;
            }
            else
            {
                ParentsComboBox.SelectedItem = _selectedItem;
            }
            

            ViewModel.IsInEdit = true;
        }

        private async void Slider_LostFocus(object sender, RoutedEventArgs e)
        {
            if (ViewModel.SelectedTodo is null || ViewModel.IsInEdit == false)
            {
                return;
            }
            await ViewModel.SelectedTodo.SaveTodoAsync();
        }

        private async void TextBox_KeyDown(object sender, KeyRoutedEventArgs e)
        {
            if (ViewModel.SelectedTodo is null || ViewModel.IsInEdit == false)
            {
                return;
            }
            
            if (e.Key == VirtualKey.Enter )
            {
                TextBox textBox = (TextBox)sender;
                if (textBox.Name == "SelectedNameEditBox")
                {
                    ViewModel.SelectedTodo.Name = textBox.Text;
                }

                else if (textBox.Name == "SelectedRewardEditBox")
                {
                    ViewModel.SelectedTodo.Reward = textBox.Text;
                }

                if (ViewModel.SelectedTodo.IsSubTodo())
                {
                    TodoViewModel _parentsTodo = ViewModel.SelectedTodo.GetParentsViewModel();
                }
                await ViewModel.SelectedTodo.SaveTodoAsync();
            }
        }

        private async void ParentsComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ViewModel.SelectedTodo is null || ViewModel.IsInEdit == false)
            {
                return;
            }

            ViewModel.SelectedTodo.ReleaseFromParentsTodo();

            /// - None -
            if (ParentsComboBox.SelectedIndex == 0)
            {
                App.AppViewModel.RootTodoViewModels.Add(ViewModel.SelectedTodo);
            }

            else
            {
                App.AppViewModel.RootTodoViewModels.Remove(ViewModel.SelectedTodo);

                TodoViewModel selectedParentTodoViewModel = ParentsComboBox.SelectedItem as TodoViewModel;
                ViewModel.SelectedTodo.SetParents(ref selectedParentTodoViewModel);
            }

            await ViewModel.SelectedTodo.SaveTodoAsync();
        }

        
    }
}