using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides a bindable wrapper for the Todo model class, encapsulating various services for access by the UI.
    /// </summary>
    public class TodoViewModel : BindableBase
    {

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Property//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private Todo _todoModel;

        /// <summary>
        /// Initializes a new instance of the JobsViewModel class that wraps a Todo object.
        /// </summary>
        public TodoViewModel(Todo todo = null)
        {
            TodoModel = todo ?? new Todo(string.Empty);
            SubTodos = new ObservableCollection<TodoViewModel>();
            //NewSubTodoView = new TodoViewModel();
        }
        
        public ObservableCollection<TodoViewModel> SubTodos { get; } = new ObservableCollection<TodoViewModel>();


        /// <summary>
        /// Gets or sets the underlying Todo object.
        /// </summary>
        public Todo TodoModel
        {
            get => _todoModel;
            set
            {
                if (_todoModel != value)
                {
                    _todoModel = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public string Reward
        {
            get => TodoModel.Reward;
            set
            {
                if (value != TodoModel.Reward)
                {
                    TodoModel.Reward = value;
                    OnPropertyChanged();
                }
            }
        }

        public bool IsTerminated
        {
            get { return TodoModel.IsTerminated; }
            set
            {
                TodoModel.IsTerminated = value;
                OnPropertyChanged();
            }
        }


        public string Name
        {
            get => TodoModel.Name;
            set
            {
                TodoModel.Name = value;
                OnPropertyChanged();
            }
        }

        public Guid Id
        {
            get => TodoModel.Id;
        }

        public Guid ParentsTodoId
        {
            get => TodoModel.ParentsTodo;
            set
            {
                if (value != TodoModel.ParentsTodo)
                {
                    TodoModel.ParentsTodo = value;
                    OnPropertyChanged();
                }
            }
        }
        
        

        private bool _isNewTodo;
        /// <summary>
        /// Gets or sets a value that indicates whether this is a new Todo.
        /// </summary>
        public bool IsNewTodo
        {
            get => _isNewTodo;
            set => Set(ref _isNewTodo, value);
        }

        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///METHOD////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////




        /// <summary>
        /// Saves todo data that has been edited.
        /// </summary>
        public async Task SaveTodoAsync()
        {
            if (IsNewTodo)
            {
                IsNewTodo = false;
                if (this.IsSubTodo())
                {
                    var _parentsTodoViewModel = this.GetParentsViewModel();
                    _parentsTodoViewModel.SubTodos.Add(this);
                }
                else
                {
                    App.AppViewModel.Todos.Add(this);
                }
            }

            await App.Repository.Todos.UpsertAsync(TodoModel);
        }

        public async Task ReleaseTodo(bool isTerminated = false)
        {
            if (isTerminated)
            {
                this.IsTerminated = isTerminated;
                await SaveTodoAsync();
            }


            if (IsSubTodo())
            {
                //TODO: CreateFromGuid create empty TodoViewModel. Find out later
                //var _parentsTodo = await TodoViewModel.CreateFromGuid(deleteTodo.ParentsTodo);
                var _parentsTodoViewModel = GetParentsViewModel();
                _parentsTodoViewModel.SubTodos.Remove(this);
            }
            else
            {
                App.AppViewModel.Todos.Remove(App.AppViewModel.Todos.Where(td => td.Id == this.Id).Single());
            }
        }

        /// <summary>
        /// Deletes the specified todo from the database.
        /// </summary>
        public async Task DeleteTodo()
        {
            await App.Repository.Todos.DeleteAsync(this.Id);
            await ReleaseTodo();
        }

        public TodoViewModel GetParentsViewModel()
        {
            foreach (var todo in App.AppViewModel.Todos)
            {
                if (ParentsTodoId == todo.Id)
                {
                    return todo;
                }
            }
            return null;
        }

        /// <summary>
        /// Creates an TodoViewModel that wraps an Todo object created from the specified Id.
        /// </summary>
        public static async Task<TodoViewModel> CreateFromGuid(Guid todoId) =>
            new TodoViewModel(await GetTodo(todoId));

        /// <summary>
        /// Returns the todo with the specified Id.
        /// It is slower than GetParentsViewModel()
        /// </summary>
        private static async Task<Todo> GetTodo(Guid todoId) =>
            await App.Repository.Todos.GetAsync(todoId);

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a customer.
        /// </summary>
        public async void EndEdit() => await SaveTodoAsync();
        

        public int CountDailyCount()
        {
            return ++TodoModel.DailyCount;
        }

        public bool CheckDailyCompleted(int soFarCount)
        {
            if (soFarCount == TodoModel.DailyCount)
            {
                TodoModel.IsDailyCompleted = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        
        public bool IsSubTodo()
        {
            if (default(Guid) != this.TodoModel.ParentsTodo)
            {
                return true;
            }
            return false;
        }
    }
}