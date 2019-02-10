using Microsoft.Toolkit.Uwp.Helpers;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{
    public class TodoListViewModel : BindableBase
    {
        //// <summary>
        /// The collection of all todos in the list. 
        /// </summary>
        public ObservableCollection<TodoViewModel> AllTodoViewModels { get; }
            = new ObservableCollection<TodoViewModel>();
        
        private TodoViewModel _selectedTodo;

        /// <summary>
        /// Gets or sets the selected t
        /// </summary>
        public TodoViewModel SelectedTodo
        {
            get => _selectedTodo;
            set => Set(ref _selectedTodo, value);
        }

        private bool _isInEdit;
        public bool IsInEdit
        {
            get => _isInEdit;
            set => Set(ref _isInEdit, value);
        }

        public async Task GetTodoListAsync()
        {
            var allTodos = await App.Repository.Todos.GetAsync();

            if (null == allTodos)
            {
                return;
            }

            // TodoViewModel
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                AllTodoViewModels.Clear();

                foreach (var pt in allTodos)
                {
                    var newTodoViewModel = new TodoViewModel(pt);
                    AllTodoViewModels.Add(newTodoViewModel);
                }
                
            });
        }
    }
}
