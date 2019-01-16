using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;
using Pomoductive.Models;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides data and commands accessible to the entire app.  
    /// </summary>
    public class ApplicationViewModel : BindableBase
    {
        /// <summary>
        /// Creates a new MainViewModel.
        /// </summary>
        public ApplicationViewModel()
        {
            Task.Run(GetTodoListAsync);
        }

        //// <summary>
        /// The collection of todos in the list. 
        /// </summary>
        public ObservableCollection<TodoViewModel> Todos { get; }
            = new ObservableCollection<TodoViewModel>();
        public StopWatchViewModel Stopwatch = new StopWatchViewModel();

        private bool _isLoading = false;

        /// <summary>
        /// Gets or sets a value indicating whether the Customers list is currently being updated. 
        /// </summary>
        public bool IsLoading
        {
            get => _isLoading;
            set
            {
                Set(ref _isLoading, value);
            }
        }

        /// <summary>
        /// Gets the complete list of Todos from the database.
        /// </summary>
        public async Task GetTodoListAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var todos = await App.Repository.Todos.GetAsync();
            if (todos == null)
            {
                return;
            }
            
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Todos.Clear();
                foreach (var c in todos)
                {
                    Todos.Add(new TodoViewModel(c));
                }
                IsLoading = false;
            });
        }

        private TodoViewModel _selectedTodo;

        /// <summary>
        /// Gets or sets the selected order.
        /// </summary>
        public TodoViewModel SelectedTodo
        {
            get => _selectedTodo;
            set => Set(ref _selectedTodo, value);
            
        }

        /// <summary>
        /// Deletes the specified Todo from the database.
        /// </summary>
        public async Task DeleteTodo(TodoViewModel orderToDelete)
        {
            await App.Repository.Todos.DeleteAsync(orderToDelete.ID);
            Todos.Remove(Todos.Where(td => td.ID == orderToDelete.ID).Single());
        }
    }
}
