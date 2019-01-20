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

        private Todo _todoModel;

        /// <summary>
        /// Initializes a new instance of the JobsViewModel class that wraps a Todo object.
        /// </summary>
        public TodoViewModel(Todo todo = null)
        {
            TodoModel = todo ?? new Todo(string.Empty);
            SubTodos = new ObservableCollection<Todo>(TodoModel.SubTodos);

            //NewSubTodoView = new TodoViewModel();
        }

        private ObservableCollection<Todo> _subTodos;
        public ObservableCollection<Todo> SubTodos
        {
            get => _subTodos;
            set
            {
                if (_subTodos != value)
                {
                    if (value != null)
                    {
                        value.CollectionChanged += SubTodos_Changed;
                    }

                    if (_subTodos != null)
                    {
                        _subTodos.CollectionChanged -= SubTodos_Changed;
                    }
                    _subTodos = value;
                    OnPropertyChanged();
                    //IsModified = true;
                }
            }
        }

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

        public Guid ParentsTodo
        {
            get => TodoModel.ParentsTodo;
            set
            {
                if (value != TodoModel.ParentsTodo)
                {
                    _todoModel.ParentsTodo = value;
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

        /// <summary>
        /// Saves todo data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            if (IsNewTodo)
            {
                IsNewTodo = false;
                App.AppViewModel.Todos.Add(this);
            }

            await App.Repository.Todos.UpsertAsync(TodoModel);
        }

        public static async Task SaveAsync(Todo todo)
        {
            await App.Repository.Todos.UpsertAsync(todo);
        }

        /// <summary>
        /// Deletes the specified todo from the database.
        /// </summary>
        public async Task DeleteTodo(Todo TodoToDelete) =>
            await App.Repository.Todos.DeleteAsync(TodoToDelete.Id);

        /// <summary>
        /// Creates an TodoViewModel that wraps an Todo object created from the specified Id.
        /// </summary>
        public static async Task<TodoViewModel> CreateFromGuid(Guid todoId) =>
            new TodoViewModel(await GetTodo(todoId));

        /// <summary>
        /// Returns the todo with the specified Id.
        /// </summary>
        private static async Task<Todo> GetTodo(Guid todoId) =>
            await App.Repository.Todos.GetAsync(todoId);

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a customer.
        /// </summary>
        public async void EndEdit() => await SaveAsync();
        

        public int CountDailyCount()
        {
            return ++_todoModel.DailyCount;
        }

        public bool CheckDailyCompleted(int soFarCount)
        {
            if (soFarCount == _todoModel.DailyCount)
            {
                _todoModel.IsDailyCompleted = true;
                return true;
            }
            else
            {
                return false;
            }
        }

        ///
        /// Sub Todo Methods /////////////////////////////////////////////////////
        /// 

        public static bool IsSubTodo(Todo todo)
        {
            if (default(Guid) != todo.ParentsTodo)
            {
                return true;
            }
            return false;
        }
        /// <summary>
        /// Notifies anyone listening to this object that a line item changed. 
        /// </summary>
        private void SubTodos_Changed(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (SubTodos != null)
            {
                TodoModel.SubTodos = SubTodos.ToList();
            }

            OnPropertyChanged(nameof(SubTodos));
            //IsModified = true;
        }

        private TodoViewModel _newSubTodoView;

        /// <summary>
        /// Gets or sets the line item that the user is currently working on.
        /// </summary>
        public TodoViewModel NewSubTodoView
        {
            get => _newSubTodoView;
            set
            {
                if (value != _newSubTodoView)
                {
                    if (value != null)
                    {
                        value.PropertyChanged += NewSubTodo_PropertyChanged;
                    }

                    if (_newSubTodoView != null)
                    {
                        _newSubTodoView.PropertyChanged -= NewSubTodo_PropertyChanged;
                    }

                    _newSubTodoView = value;
                    UpdateNewSubTodoBindings();
                }
            }
        }

        private void NewSubTodo_PropertyChanged(object sender, PropertyChangedEventArgs e) => UpdateNewSubTodoBindings();

        private void UpdateNewSubTodoBindings()
        {
            OnPropertyChanged(nameof(NewSubTodoView));
        }
    }
}