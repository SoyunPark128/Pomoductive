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

            var parentsTodos = await App.Repository.Todos.GetForParentsTodoAsync();
            var SubTodos = await App.Repository.Todos.GetForSubTodoAsync();

            if (null == parentsTodos)
            {
                return;
            }

            // Todo
            foreach (var subTodo in SubTodos)
            {
                var parentsWhichHasSubs =  parentsTodos.ToList<Todo>().Find(x => x.Id == subTodo.ParentsTodo);
                if (null != parentsWhichHasSubs)
                {
                    parentsWhichHasSubs.SubTodos.Add(subTodo);
                    subTodo.ParentsTodo = parentsWhichHasSubs.Id;
                }
            }

            // TodoViewModel
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                Todos.Clear();
                foreach (var c in parentsTodos)
                {
                    var newTodoViewModel = new TodoViewModel(c);
                    Todos.Add(newTodoViewModel);
                }
                IsLoading = false;
            });
        }

        /// <summary>
        /// Deletes the specified Todo from the database.
        /// </summary>
        public async Task DeleteTodo(Todo deletedTodo)
        {
            await App.Repository.Todos.DeleteAsync(deletedTodo.Id);
            await ReleaseTodo(deletedTodo);
        }

        public async Task ReleaseTodo(Todo releasedTodo, bool isTerminated = false)
        {
            if (isTerminated)
            {
                releasedTodo.IsTerminated = isTerminated;
                await TodoViewModel.SaveAsync(releasedTodo);
            }


            if (TodoViewModel.IsSubTodo(releasedTodo))
            {
                //TODO: CreateFromGuid create empty TodoViewModel. Find out later
                var _parentsTodoViewModel = GetParents(releasedTodo.ParentsTodo);
                //var _parentsTodo = await TodoViewModel.CreateFromGuid(deleteTodo.ParentsTodo);
                _parentsTodoViewModel.SubTodos.Remove(releasedTodo);
            }
            else
            {
                Todos.Remove(Todos.Where(td => td.Id == releasedTodo.Id).Single());
            }
        }
        
        public TodoViewModel GetParents(Guid parentsID)
        {
            foreach (var todo in Todos)
            {
                if (parentsID == todo.Id)
                {
                    return todo;
                }
            }
            return null;
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

        private ObservableCollection<ShellNavigationItem> _navigationItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> NavigationItems
        {
            get { return _navigationItems; }
            set { Set(ref _navigationItems, value); }
        }
    }
}
