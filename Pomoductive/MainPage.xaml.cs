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
        }

        public TodoViewModel ViewModel = new TodoViewModel();


        private async Task Button_ClickAsync(object sender, RoutedEventArgs e)
        {
            Todo newCategory = new Todo(TaskNameInput.Text);

            ViewModel = new TodoViewModel(newCategory)
            {
                Reward = "Sleep"
            };


            CheckBox taskCheckBox = new CheckBox();
            taskCheckBox.Name = "Task" + newCategory.Name;
            taskCheckBox.Content = newCategory.Name;
            taskCheckBox.Checked += Task_Finished_Check;
            

            TaskListPanel.Children.Add(taskCheckBox);
            TaskNameInput.ClearValue(TextBox.TextProperty);
            
            await ViewModel.SaveAsync();
            testText.Text = App.ViewModel.Todos.Count.ToString() ?? "Nothing";
            

        }

        private void Task_Finished_Check(object sender, RoutedEventArgs e)
        {

        }

        
    }
}
