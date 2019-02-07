using Pomoductive.ViewModels;
using System;
using System.Globalization;
using System.Linq;
using Windows.UI.Xaml.Controls;
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
        

        public static double GetEllipseSize(float value)
        {
            double _ellipseSize = value*5 + 10;
            if (_ellipseSize > 80)
            {
                _ellipseSize = 80;
            }
            return _ellipseSize;
        }

        public static string GetPomoCount(float value)
        {
            string _pomoCount = value.ToString("F1", CultureInfo.InvariantCulture) + "Pomos";
            return _pomoCount;
        }

        public static double GetCenterPoint(float value)
        {
            return (90 - GetEllipseSize(value)) / 2;
        }

        public static string GetNameFromID(Guid id)
        {
            TodoViewModel _todoViewModel = App.AppViewModel.TodoViewModels.FirstOrDefault(x => x.Id == id);
            return _todoViewModel is null ? "" : _todoViewModel.Name;
        }

        public static string TerminatedToString(bool isterminated)
        {
            return isterminated ? "Terminated" : "In Process";
        }

        public static bool SetOppositeBoolean(bool value)
        {
            return !value;
        }
    }
}
