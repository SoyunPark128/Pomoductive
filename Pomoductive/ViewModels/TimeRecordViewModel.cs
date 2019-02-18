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
    /// Provides a bindable wrapper for the TimeRecordModel model class, encapsulating various services for access by the UI.
    /// </summary>
    public class TimeRecordViewModel : BindableBase
    {
        private TimeRecord _timeRecord;

        public TimeRecordViewModel()
        { }
        /// <summary>
        /// Initializes a new instance of the TimeRecordModel class that wraps a Todo object.
        /// </summary>
        public TimeRecordViewModel(Guid todoId, string todoName)
        {
            TimeRecordModel = new TimeRecord(todoId, todoName);
        }

        public TimeRecordViewModel(TimeRecord timeRecord)
        {
            TimeRecordModel = timeRecord;
        }

        /// <summary>
        /// Gets or sets the underlying TimeRecordModel object.
        /// </summary>
        public TimeRecord TimeRecordModel
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

        public Guid Id
        {
            get => _timeRecord.Id;
        }

        public Guid TodoId
        {
            get => TimeRecordModel.TodoId;
            set
            {
                TimeRecordModel.TodoId = value;
                OnPropertyChanged();
            }
        }

        public string TodoName
        {
            get => TimeRecordModel.TodoName;
            set
            {
                TimeRecordModel.TodoName = value;
                OnPropertyChanged();
            }
        }

        public DateTime RedordingDate { get => TimeRecordModel.RedordingDate; }

        public int TaskMin
        {
            get => TimeRecordModel.TaskMin;
            set
            {
                TimeRecordModel.TaskMin = value;
                OnPropertyChanged();
            }
        }

        public int TotalTaskCount
        {
            get => TimeRecordModel.TotalTaskCount; 
            set
            {
                TimeRecordModel.TotalTaskCount = value;
                OnPropertyChanged();
            }
        }

        public float Remainder
        {
            get => TimeRecordModel.Remainder;
            set
            {
                TimeRecordModel.Remainder = value;
                OnPropertyChanged();
            }
        }

        public float TotalTaskCountAccurate
        {
            get => TotalTaskCount + Remainder;
        }

        public async Task SaveTimeRecordAsync()
        {
            await App.Repository.TimeRecords.UpsertAsync(TimeRecordModel);
        }
    }
}
