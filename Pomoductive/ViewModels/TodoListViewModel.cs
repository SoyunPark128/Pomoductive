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
        public ObservableCollection<TodoViewModel> AllTodoViewModels => App.AppViewModel.TodoViewModels;

        public ObservableCollection<TodoViewModel> ParrentsTodos { get; set; } = new ObservableCollection<TodoViewModel>();


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

        public void GetParentsTodoList()
        {
            ParrentsTodos.Clear();

            var defaultNoneParentsTodoViewModel = new TodoViewModel();
            defaultNoneParentsTodoViewModel.Name = " - None - ";
            ParrentsTodos.Add(defaultNoneParentsTodoViewModel);

            foreach (var todo in AllTodoViewModels)
            {
                if (SelectedTodo.Id != todo.Id)
                {
                    ParrentsTodos.Add(todo);
                }
            }
        }
    }
}
