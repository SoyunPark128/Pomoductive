using Pomoductive.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides a bindable wrapper for the Job model class, encapsulating various services for access by the UI.
    /// </summary>
    public class JobsViewModel
    {
        
        /// <summary>
        /// Initializes a new instance of the JobsViewModel class that wraps a Customer object.
        /// </summary>
        public JobsViewModel(Job job = null)
        {
            JobModel = job ?? new Job(string.Empty);
        }

        private Job _jobModel;

        public event PropertyChangedEventHandler PropertyChanged;

        /// <summary>
        /// Gets or sets the underlying Job object.
        /// </summary>
        public Job JobModel
        {
            get => _jobModel;
            set
            {
                if (_jobModel != value)
                {
                    _jobModel = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        /// <summary>
        /// Notifies listeners that a property value has changed.
        /// </summary>
        /// <param name="propertyName">Name of the property used to notify listeners. This
        /// value is optional and can be provided automatically when invoked from compilers
        /// that support <see cref="CallerMemberNameAttribute"/>.</param>
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null) =>
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));


        /// <summary>
        /// Saves job data that has been edited.
        /// </summary>
        public async Task SaveAsync()
        {
            App.ViewModel.Jobs.Add(this);

        }

    }
}
