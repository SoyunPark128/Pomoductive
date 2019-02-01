using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Repository
{
    public interface IJournalRepository
    {
        /// <summary>
        /// Returns all Journals. 
        /// </summary>
        Task<IEnumerable<Journal>> GetAsync();

        /// <summary>
        /// Returns the Journal with the given id. 
        /// </summary>
        Task<Journal> GetAsync(Guid id);
        
        /// <summary>
        /// Returns the Journal with the given Date. 
        /// </summary>
        Task<Journal> GetAsyncByDate(DateTime redordingDate);

        /// <summary>
        /// Adds a new Journal if the Journal does not exist, updates the 
        /// existing Journal otherwise.
        /// </summary>
        Task<Journal> UpsertAsync(Journal journal);

        /// <summary>
        /// Deletes a Journal.
        /// </summary>
        Task DeleteAsync(Guid Id);

    }
}
