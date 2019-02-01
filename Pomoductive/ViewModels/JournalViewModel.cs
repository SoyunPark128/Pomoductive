using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.ViewModels
{
    /// <summary>
    /// Provides a bindable wrapper for the Journal model class, encapsulating various services for access by the UI.
    /// </summary>
    public class JournalViewModel : BindableBase
    {

        public JournalViewModel(Journal journal = null)
        {
            JournalModel = journal ?? new Journal();
        }

        private Journal _jornalModel;

        /// <summary>
        /// Gets or sets the underlying Todo object.
        /// </summary>
        public Journal JournalModel
        {
            get => _jornalModel;
            set
            {
                if (_jornalModel != value)
                {
                    _jornalModel = value;
                    // Raise the PropertyChanged event for all properties.
                    OnPropertyChanged(string.Empty);
                }
            }
        }

        public string JournalContents
        {
            get => JournalModel.JournalContents;
            set
            {
                if (value != JournalModel.JournalContents)
                {
                    JournalModel.JournalContents = value;
                    OnPropertyChanged();
                }
            }
        }

        public DateTime JournalDate
        {
            get => JournalModel.JournalDate; set
            {
                if (value != JournalModel.JournalDate)
                {
                    JournalModel.JournalDate = value;
                    OnPropertyChanged();
                }
            }
        }
        public async Task SaveJournalkAsync()
        {
            await App.Repository.Journals.UpsertAsync(JournalModel);
        }
    }
}
