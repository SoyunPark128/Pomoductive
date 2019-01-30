using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;
using static Pomoductive.ViewModels.StatisticDataViewModel;

namespace Pomoductive
{
    public static class Converters
    {
        // Define the Convert method to convert a DateTime value to 
        // a month string.
        public static TreeViewSelectionMode IsRunningToSelectionMode(bool value)
        {
            // value is the data from the source object.
            TreeViewSelectionMode SelectionMode = value ? TreeViewSelectionMode.None : TreeViewSelectionMode.Single;

            return SelectionMode;
        }

        public static TimeSpan RemainderToRemainTime(float remainer, int taskMin)
        {
            return TimeSpan.FromMinutes(taskMin - taskMin * remainer);
        }

        public static float RemainTimeToRemainder(TimeSpan remainTime, int taskMin)
        {
            return (taskMin - (float)remainTime.TotalMinutes) / taskMin;
        }

        public static ObservableCollection<TodosPerOneday> TodosPerADayDicToStruct(Dictionary<string, int>dic, DateTime start, DateTime end)
        {
            var TodosPerADayStructCollection = new ObservableCollection<TodosPerOneday>();
            foreach (var item in dic)
            {
                if (DateTime.Parse(item.Key) >= start && DateTime.Parse(item.Key) < end)
                {
                    TodosPerOneday _todosInOneday = new TodosPerOneday();
                    _todosInOneday.Date = item.Key;
                    _todosInOneday.Todos = item.Value;
                    TodosPerADayStructCollection.Add(_todosInOneday);
                }
                
            }
            return TodosPerADayStructCollection;
        }

        public static double GetEllipseSize(float value)
        {
            return (double)(value * 5);
        }
    }
}
