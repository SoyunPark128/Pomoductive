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
    public class TodoViewModel
    {

        private Todo _todoModel;
        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Initializes a new instance of the JobsViewModel class that wraps a Customer object.
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

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        /// <summary>
        /// Saves todo data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            App.ViewModel.Todos.Add(this);

        }

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