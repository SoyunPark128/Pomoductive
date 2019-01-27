using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Data;

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

    }
}
