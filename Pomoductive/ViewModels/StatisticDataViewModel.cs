using Microsoft.Toolkit.Uwp.Helpers;
using Pomoductive.Models;
using Pomoductive.Views;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;

namespace Pomoductive.ViewModels
{

    public class StatisticDataViewModel:BindableBase
    {
        
        public ObservableCollection<TimeRecordViewModel> TimeRecordViewModels = new ObservableCollection<TimeRecordViewModel>();

        #region Statistic Datas
        // For Main Page Graph
        public ObservableDictionary<string, double> GraphDataDicTotalTodosPerADay = new ObservableDictionary<string, double>();
        public ObservableDictionary<string, double> GraphDataDicTotalTodosPerADay2Weeks = new ObservableDictionary<string, double>();

        // For Journal Page
        public ObservableCollection<TimeRecordViewModel> GraphDataTotalTodosAndNamePerADay = new ObservableCollection<TimeRecordViewModel>();

        // For Statistic Page
        public ObservableDictionary<string, double> GraphDataDicTodoPortions = new ObservableDictionary<string, double>();
        public ObservableDictionary<string, double> GraphDataDicNumOfPomosOnTime = new ObservableDictionary<string, double>();
        public ObservableDictionary<string, double> GraphDataDicTotalTodosPerADaySpecificPeriod = new ObservableDictionary<string, double>();
        #endregion

        public async Task ConfigureStatisticsDatas()
        {
            
            //Load Datas to TimeRecordsFor2Weeks
            Task timerecordSetup = Task.Run(GetTimeRecordDatasAsync);
            await timerecordSetup.ContinueWith(
                delegate { LoadStatisticData(); }).ContinueWith(
                delegate { SetTotalTodosAndNamePerADay(DateTime.Today); });
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
        public async Task GetTimeRecordDatasAsync()
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
            });
        }

        private void LoadStatisticData()
        {
            if (TimeRecordViewModels is null)
            {
                return;
            }
            
            // Sort by Hour
            for (int t = 0; t < 25; t++)
            {
                GraphDataDicNumOfPomosOnTime.Add(t.ToString(), 0);
            }

            foreach (var trvm in TimeRecordViewModels)
            {
                string _date = trvm.RedordingDate.ToShortDateString();
                string _hour = trvm.RedordingDate.Hour.ToString();
                string _name = trvm.TodoName;
                double _count = trvm.TotalTaskCount + trvm.Remainder;



                //GraphDataTotalTodosPerADay
                try
                {
                    GraphDataDicTotalTodosPerADay[_date] += _count;
                }
                catch (KeyNotFoundException)
                {
                    GraphDataDicTotalTodosPerADay.Add(_date, _count);
                }

                //GraphDataDicTodoPortions
                try
                {
                    GraphDataDicTodoPortions[_name] += _count;
                }
                catch (KeyNotFoundException)
                {
                    GraphDataDicTodoPortions.Add(_name, _count);
                }

                // GraphDataDicNumOfPomos
                try
                {
                    GraphDataDicNumOfPomosOnTime[_hour] += _count;
                }
                catch (KeyNotFoundException)
                {

                    GraphDataDicNumOfPomosOnTime.Add(_hour, _count);
                }
            }

            DataDicFilter(GraphDataDicTotalTodosPerADay, ref GraphDataDicTotalTodosPerADay2Weeks, DateTime.Now.AddDays(-14), DateTime.Now,"Date");
        }

        public void DataDicFilter(ObservableDictionary<string, double> orinignalDataDic, ref ObservableDictionary<string, double>targetDataDic, 
            DateTime start, DateTime end, string keyType, string parentsTodoName = "")
        {

            /// TODO: Now it only filters Date-Count Dictionary.
            targetDataDic.Clear();

            for (DateTime d = start; d < end;)
            {
                if (orinignalDataDic.TryGetValue(d.ToShortDateString(), out double _value))
                {
                    switch (keyType)
                    {
                        case "Date":
                            targetDataDic.Add(d.ToShortDateString(), _value);
                            break;
                        case "Name":
                            break;
                        default:
                            break;
                    }
                }
                else
                {
                    switch (keyType)
                    {
                        case "Date":
                            targetDataDic.Add(d.ToShortDateString(), _value);
                            break;
                        case "Name":
                            break;
                        default:
                            break;
                    }
                }
                d = d.AddDays(1);
            }
            
        }

        private void StatisticsDataUpdate(object sender, NotifyCollectionChangedEventArgs args)
        {
            double before = 0;
            foreach (var tad in GraphDataDicTotalTodosPerADay2Weeks)
            {
                if (tad.Key == DateTime.Today.ToShortDateString())
                {
                    before = tad.Value;
                    GraphDataDicTotalTodosPerADay2Weeks.Remove(tad);
                    break;
                }
            }
            GraphDataDicTotalTodosPerADay2Weeks.Add(DateTime.Today.ToShortDateString(), ++before);
        }

        // For Journal
        public void SetTotalTodosAndNamePerADay(DateTime date)
        {

            GraphDataTotalTodosAndNamePerADay.Clear();
            foreach (var item in TimeRecordViewModels)
            {
                if (item.RedordingDate == date)
                {
                    GraphDataTotalTodosAndNamePerADay.Add(item);
                }
            }
        }
    }
}
