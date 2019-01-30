using Microsoft.Toolkit.Uwp.Helpers;
using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{

    public class StatisticDataViewModel:BindableBase
    {
        public ObservableCollection<TimeRecordViewModel> TimeRecordViewModels = new ObservableCollection<TimeRecordViewModel>();
        private Dictionary<string, int> TotalTodosPerADayDic = new Dictionary<string, int>();

        public ObservableCollection<TodosPerOneday> TotalTodosPerADay = new ObservableCollection<TodosPerOneday>();

        public StatisticDataViewModel()
        {
            //Load Datas to TimeRecordsFor2Weeks
            Task.Run(GetTimeRecordDatasAsync);
            TimeRecordViewModels.CollectionChanged += new NotifyCollectionChangedEventHandler(StatisticsDataUpdate);

            
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
        /// Gets the Journals from the database.
        /// </summary>
        private async Task GetTimeRecordDatasAsync()
        {
            await DispatcherHelper.ExecuteOnUIThreadAsync(() => IsLoading = true);

            var TimeRecords = await App.Repository.TimeRecords.GetAsync();

            if (null == TimeRecords)
            {
                return;
            }

            // TodoViewModel
            await DispatcherHelper.ExecuteOnUIThreadAsync(() =>
            {
                TimeRecordViewModels.Clear();

                foreach (var tr in TimeRecords)
                {
                    var newTimeRecordViewModel = new TimeRecordViewModel(tr);
                    TimeRecordViewModels.Add(newTimeRecordViewModel);
                }
                IsLoading = false;
                LoadTotalTodosInOneDay();
                
                // For the MainPage
                TotalTodosPerADay = Converters.TodosPerADayDicToStruct(TotalTodosPerADayDic, DateTime.Now.AddDays(-14), DateTime.Now);
            });
            

        }

        private void LoadTotalTodosInOneDay()
        {
            if (TimeRecordViewModels is null)
            {
                return;
            }

            foreach (var trvm in TimeRecordViewModels)
            {
                string _date = trvm.RedordingDate.ToShortDateString();
                int _count = trvm.TotalTaskCount;

                try
                {
                    TotalTodosPerADayDic[_date] += _count;
                }
                catch (KeyNotFoundException)
                {
                    TotalTodosPerADayDic[_date] = _count;
                }
            }
        }

        private void StatisticsDataUpdate(object sender, System.Collections.Specialized.NotifyCollectionChangedEventArgs args)
        {
            int before = 0;
            foreach (var tad in TotalTodosPerADay)
            {
                if (tad.Date == DateTime.Today.ToShortDateString())
                {
                    before = tad.Todos;
                    TotalTodosPerADay.Remove(tad);
                    break;
                }
            }
            TodosPerOneday _todosInOneday = new TodosPerOneday();
            _todosInOneday.Date = DateTime.Today.ToShortDateString();
            _todosInOneday.Todos = ++before;
            TotalTodosPerADay.Add(_todosInOneday);
        }

        public struct TodosPerOneday
        {
            public string Date { get; set; }
            public int Todos { get; set; }
        }
    }
}
