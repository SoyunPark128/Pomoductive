using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{

    public class StatisticDataViewModel
    {
        public ObservableCollection<TimeRecordViewModel> TimeRecordViewModelsFor2Weeks = new ObservableCollection<TimeRecordViewModel>();
        public ObservableCollection<TodosPerOneday> TotalTodosPerADay2Weeks = new ObservableCollection<TodosPerOneday>();
        private Dictionary<string, int> TotalTodosPerADay2WeeksDic = new Dictionary<string, int>();

        public StatisticDataViewModel()
        {
            //Load Datas to TimeRecordsFor2Weeks
            Task.Run(LoadTimeRecordViewModelsFor2WeeksData);
        }
        
        private async Task LoadTimeRecordViewModelsFor2WeeksData()
        {
            DateTime Day2WeekAgo = DateTime.Now.AddDays(-14);
            var TimeRecordsFor2Weeks = await App.Repository.TimeRecords.GetAsyncByDate(Day2WeekAgo, DateTime.Today);
            foreach (var TimeRecord in TimeRecordsFor2Weeks)
            {
                var newTimeRecordViewModel = new TimeRecordViewModel(TimeRecord);
                TimeRecordViewModelsFor2Weeks.Add(newTimeRecordViewModel);
            }
            LoadTotalTodosInOneDay();
            TotalTodosPerADay2Weeks = Converters.TodosPerADayDicToStruct(TotalTodosPerADay2WeeksDic);
        }


        private void LoadTotalTodosInOneDay()
        {
            if (TimeRecordViewModelsFor2Weeks is null)
            {
                return;
            }

            foreach (var trvm in TimeRecordViewModelsFor2Weeks)
            {
                string _date = trvm.RecordingDay.ToShortDateString();
                int _count = trvm.TotalTaskCount;

                try
                {
                    TotalTodosPerADay2WeeksDic[_date] += _count;
                }
                catch (KeyNotFoundException)
                {
                    TotalTodosPerADay2WeeksDic[_date] = _count;
                }
                
            }
        }


        public struct TodosPerOneday
        {
            public string Date { get; set; }
            public int Todos { get; set; }
        }
    }
}
