using Microsoft.Toolkit.Uwp.Helpers;
using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides a bindable wrapper for the TimeRecord model class, encapsulating various services for access by the UI.
    /// </summary>
    public class TimeRecordViewModel : BindableBase
    {
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////
        ///Property//////////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////////////////////

        private TimeRecord _timeRecord;

        public TimeRecordViewModel()
        { }
        /// <summary>
        /// Initializes a new instance of the JobsViewModel class that wraps a Todo object.
        /// </summary>
        public TimeRecordViewModel(Guid todoId)
        {
            TimeRecord = new TimeRecord(todoId);
        }

        public TimeRecordViewModel(TimeRecord timeRecord)
        {
            TimeRecord = timeRecord;
        }
        /// <summary>
        /// Gets or sets the underlying Todo object.
        /// </summary>
        public TimeRecord TimeRecord
        {
            get => _timeRecord;
            set
            {
                if (_timeRecord != value)
                {
                    _timeRecord = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public Guid TodoId
        {
            get => TimeRecord.TodoId;
            set
            {
                TimeRecord.TodoId = value;
                OnPropertyChanged();
            }
        }

        public DateTime RecordingDay { get => TimeRecord.RecordingDay; }

        public int TaskMin
        {
            get => TimeRecord.TaskMin;
            set
            {
                TimeRecord.TaskMin = value;
                OnPropertyChanged();
            }
        }

        public int TotalTaskCount
        {
            get => TimeRecord.TotalTaskCount; 
            set
            {
                TimeRecord.TotalTaskCount = value;
                OnPropertyChanged();
            }
        }

        public float Remainder
        {
            get => TimeRecord.Remainder;
            set
            {
                TimeRecord.Remainder = value;
                OnPropertyChanged();
            }
        }

        public async Task SaveTimeRecordAsync()
        {
            await App.Repository.TimeRecords.UpsertAsync(TimeRecord);
        }
    }
}
