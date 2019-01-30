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
        /// Initializes a new instance of the TodoViewModel class that wraps a Todo object.
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
            get => TodoModel.IsTerminated;
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

        public int DailyCount
        {
            get => TodoModel.DailyCount;
            set
            {
                if (value != TodoModel.DailyCount)
                {
                    TodoModel.DailyCount = value;
                    OnPropertyChanged();
                }
            }
        }
        
        public int TaskMinutesPerOnePomo
        {
            get => TodoModel.TaskMinutesPerOnePomo;
            set
            {
                if (value != TodoModel.TaskMinutesPerOnePomo)
                {
                    TodoModel.TaskMinutesPerOnePomo = value;
                    OnPropertyChanged();
                }
            }
        }

        public int ShortBreakMinutesPerOnePomo
        {
            get => TodoModel.ShortBreakMinutesPerOnePomo;
            set
            {
                if (value != TodoModel.ShortBreakMinutesPerOnePomo)
                {
                    TodoModel.ShortBreakMinutesPerOnePomo = value;
                    OnPropertyChanged();
                }
            }
        }

        public int LongBreakMinutesPerOnePomo
        {
            get => TodoModel.LongBreakMinutesPerOnePomo;
            set
            {
                if (value != TodoModel.LongBreakMinutesPerOnePomo)
                {
                    TodoModel.LongBreakMinutesPerOnePomo = value;
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
        

        public TodoViewModel SelectedSubTodo
        {
            get { return App.AppViewModel.SelectedTodo; }
            set { App.AppViewModel.SelectedTodo = value; }
        }

        public ICollection<TimeRecord> TimeRecords
        {
            get => TodoModel.TimeRecords;
            set
            {
                if (value != TodoModel.TimeRecords)
                {
                    TodoModel.TimeRecords = value;
                    OnPropertyChanged();
                }
            }
        }

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
                    App.AppViewModel.TodoViewModels.Add(this);
                }
            }

            await App.Repository.Todos.UpsertAsync(TodoModel);
        }

        public async Task ReleaseTodo(bool isTerminated = false)
        {
            if (isTerminated)
            {
                this.IsTerminated = isTerminated;
                foreach (var st in SubTodos.ToList())
                {
                    await st.ReleaseTodo(true);
                }
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
                
                App.AppViewModel.TodoViewModels.Remove(this);
            }
        }

        /// <summary>
        /// Deletes the specified todo from the database.
        /// </summary>
        public async Task DeleteTodo()
        {
            foreach (var st in SubTodos.ToList())
            {
                await st.DeleteTodo();
            }
            await App.Repository.Todos.DeleteAsync(this.Id);
            await ReleaseTodo();
        }

        public TodoViewModel GetParentsViewModel()
        {
            foreach (var todo in App.AppViewModel.TodoViewModels)
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

        public TimeRecordViewModel GetTimeRecordViewModel()
        {
            TimeRecord _timeRecord = TimeRecords.FirstOrDefault(trvm => trvm.TodoId == Id 
                                                                            && trvm.TaskMin == TaskMinutesPerOnePomo 
                                                                            && trvm.RedordingDate == DateTime.Today);
            
            if (_timeRecord is null)
            {
                _timeRecord = new TimeRecord(Id, Name);
                _timeRecord.TaskMin = TaskMinutesPerOnePomo;
                TimeRecords.Add(_timeRecord);
            }

            TimeRecordViewModel _timeRecordViewModel = new TimeRecordViewModel(_timeRecord);
            return _timeRecordViewModel;
        }
        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a todo.
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