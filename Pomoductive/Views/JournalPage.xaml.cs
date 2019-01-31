using Pomoductive.ViewModels;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
using Windows.UI.Xaml.Shapes;

// The Blank Page item template is documented at https://go.microsoft.com/fwlink/?LinkId=234238

namespace Pomoductive.Views
{
   

    /// <summary>
    /// An empty page that can be used on its own or navigated to within a Frame.
    /// </summary>
    public sealed partial class JournalPage : Page
    { 
        StatisticDataViewModel StatisticViewModel => App.AppStatisticDataViewModel;
        JournalViewModel JournalViewModel = new JournalViewModel();
        public JournalPage()
        {
            this.InitializeComponent();
            FlyoutCalendarDatePicker.Date = DateTimeOffset.Now;
        }

        private void CalendarView_CalendarViewDayItemChanging(CalendarView sender,
                                   CalendarViewDayItemChangingEventArgs args)
        {
            
            // Render basic day items.
            if (args.Phase == 0)
            {
                // Register callback for next phase.
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
            }
            // Set blackout dates.
            else if (args.Phase == 1)
            {
                // Blackout dates in the future
                if (args.Item.Date > DateTimeOffset.Now )
                {
                    args.Item.IsBlackout = true;
                }
                // Register callback for next phase.
                args.RegisterUpdateCallback(CalendarView_CalendarViewDayItemChanging);
                
            }
        }
        
        private void CalendarFlyoutOpen(object sender, RoutedEventArgs e)
        {
            if (Windows.Foundation.Metadata.ApiInformation.IsPropertyPresent("Windows.UI.Xaml.FrameworkElement", "AllowFocusOnInteraction"))
            {
                DatePicker.AllowFocusOnInteraction = true;
            }
            FlyoutBase.ShowAttachedFlyout((FrameworkElement)sender);
        }

        private async Task JournalSaveButtonClicked(object sender, RoutedEventArgs e)
        {
            string value = string.Empty;
            JournalContentsBox.TextDocument.GetText(Windows.UI.Text.TextGetOptions.AdjustCrlf, out value);
            JournalViewModel.JournalContents = value;
            await JournalViewModel.SaveJournalkAsync();
            JournalContentsBox.Document.SetText(Windows.UI.Text.TextSetOptions.None, string.Empty);
        }

        private void FlyoutCalendarDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            App.AppStatisticDataViewModel.SetTotalTodosAndNamePerADay(args.NewDate.Value.Date);
        }
    }
}
