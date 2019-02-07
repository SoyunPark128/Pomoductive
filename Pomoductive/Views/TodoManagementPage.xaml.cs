using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
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

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pomoductive.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class TodoManagementPage : Page
    {
        ObservableCollection<TodoViewModel> ViewModel => App.AppViewModel.AllTodoViewModels;
        TodoViewModel _selectedTodo = new TodoViewModel();

        public TodoManagementPage()
        {
            this.InitializeComponent();
            _selectedTodo = new TodoViewModel();
        }

        private void Listview_ItemClick(object sender, ItemClickEventArgs e)
        {
            
            _selectedTodo = e.ClickedItem as TodoViewModel;
            _selectedTodo.OnPropertyChanged();

        }
        
    }
}