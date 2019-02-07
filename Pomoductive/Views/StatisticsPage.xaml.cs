using Microsoft.Toolkit.Uwp.UI.Controls;
using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Controls;
using Windows.UI.Xaml.Controls.Primitives;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Input;
using Windows.UI.Xaml.Media;
using Windows.UI.Xaml.Navigation;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pomoductive.Views
{
    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class StatisticsPage : Page
    {
        StatisticDataViewModel StatisticViewModel => App.AppStatisticDataViewModel;
        bool isSelectedStartAndEnd = false;

        public StatisticsPage()
        {
            this.InitializeComponent();
        }

        public void SetFilter(object sender, NotifyCollectionChangedEventArgs args)
        {
            StatisticViewModel.GraphDataDicTotalTodosPerADaySpecificPeriod.Clear();
            StatisticViewModel.DataDicFilter(StatisticViewModel.GraphDataDicTotalTodosPerADay, ref StatisticViewModel.GraphDataDicTotalTodosPerADaySpecificPeriod,
                StartDatePicker.Date.Date, EndDatePicker.Date.Date, "Date");
            
            GraphTodosInPeriod.Visibility = Visibility.Visible;
        }
        
        private void DatePicker_DateChanged(object sender, DatePickerValueChangedEventArgs e)
        {
            if (isSelectedStartAndEnd)
            {
                SetFilter(sender, null);
            }
            else
            {
                isSelectedStartAndEnd = true;
            }
        }
        
    }
}
