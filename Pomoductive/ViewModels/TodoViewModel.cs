using Pomoductive.Models;
using System;
using System.Collections.Generic;
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

        public string Name
        {
            get => TodoModel.Name;
        }

        public Guid ID
        {
            get => TodoModel.Id;
        }


        /// <summary>
        /// Saves todo data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            App.AppViewModel.Todos.Add(this);
            await App.Repository.Todos.UpsertAsync(TodoModel);
        }

        /// <summary>
        /// Deletes the specified todo from the database.
        /// </summary>
        public async Task DeleteOrder(Todo TodoToDelete) =>
            await App.Repository.Todos.DeleteAsync(TodoToDelete.Id);

        /// <summary>
        /// Called when a bound DataGrid control commits the edits that have been made to a customer.
        /// </summary>
        public async void EndEdit() => await SaveAsync();

        public void MarkTerminated()
        {
            _todoModel.IsTerminated = !_todoModel.IsTerminated;
        }

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

        public void AddSubTodo(Todo subTodo)
        {
            _todoModel.SubTodos.Add(subTodo);
        }

        public List<Todo> GetSubTodos()
        {
            return _todoModel.SubTodos;
        }
        
    }
}