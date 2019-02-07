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
        /// The collection of parents todos in the list. 
        /// </summary>
        public ObservableCollection<TodoViewModel> TodoViewModels { get; }
            = new ObservableCollection<TodoViewModel>();

        //// <summary>
        /// The collection of all todos in the list. 
        /// </summary>
        public ObservableCollection<TodoViewModel> AllTodoViewModels { get; }
            = new ObservableCollection<TodoViewModel>();

        //// <summary>
        /// The collection of journals.
        /// </summary>
        public ObservableCollection<JournalViewModel> JournalViewModels { get; }
            = new ObservableCollection<JournalViewModel>();

        //StopWatch for Entire App
        public StopWatchViewModel AppStopwatchViewModel = new StopWatchViewModel();

        private TimeRecordViewModel _appTimeRecordViewModel = new TimeRecordViewModel();
        public TimeRecordViewModel AppTimeRecordViewModel
        {
            get => _appTimeRecordViewModel;
            set
            {
                Set(ref _appTimeRecordViewModel, value);
                OnPropertyChanged();
            }
        }

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
        /// Gets the complete list of TodoViewModels from the database.
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
            
            // TodoViewModel
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                TodoViewModels.Clear();

                foreach (var pt in parentsTodos)
                {
                    var newTodoViewModel = new TodoViewModel(pt);
                    newTodoViewModel.SubTodos.Clear();
                    var subTodos = SubTodos.ToList<Todo>().FindAll(x => x.ParentsTodo == newTodoViewModel.Id);
                    foreach (var st in subTodos)
                    {
                        var newSubTodoViewModel = new TodoViewModel(st);
                        newTodoViewModel.SubTodos.Add(newSubTodoViewModel);
                        AllTodoViewModels.Add(newSubTodoViewModel);
                    }
                    TodoViewModels.Add(newTodoViewModel);
                    AllTodoViewModels.Add(newTodoViewModel);
                }
                
                
                IsLoading = false;
            });
        }

        /// <summary>
        /// Gets the Journals from the database.
        /// </summary>
        public async Task GetJournalsAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var journals = await App.Repository.Journals.GetAsync();

            if (null == journals)
            {
                return;
            }

            // TodoViewModel
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                TodoViewModels.Clear();

                foreach (var j in journals)
                {
                    var newJournalViewModel = new JournalViewModel(j);
                    JournalViewModels.Add(newJournalViewModel);
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

        private ObservableCollection<ShellNavigationItem> _navigationItems = new ObservableCollection<ShellNavigationItem>();
        public ObservableCollection<ShellNavigationItem> NavigationItems
        {
            get { return _navigationItems; }
            set { Set(ref _navigationItems, value); }
        }
    }
}
