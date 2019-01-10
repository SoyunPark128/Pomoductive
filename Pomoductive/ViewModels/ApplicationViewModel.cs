using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Toolkit.Uwp.Helpers;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides data and commands accessible to the entire app.  
    /// </summary>
    public class ApplicationViewModel
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

        /// <summary>
        /// Gets the complete list of Todos from the database.
        /// </summary>
        public async Task GetTodoListAsync()
        {
            /*
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var Todos = await App.Repository.Todos.GetAsync();
            if (Todos == null)
            {
                return;
            }
            
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Todos.Clear();
                foreach (var c in Todos)
                {
                    Todos.Add(new TodoViewModel(c));
                }
                IsLoading = false;*
            });
            */
        }
    }
}
