using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;

namespace Pomoductive.Repository
{
    public interface ITimeRecordRepository
    {
        /// <summary>
        /// Returns all TimeRecords. 
        /// </summary>
        Task<IEnumerable<TimeRecord>> GetAsync();

        /// <summary>
        /// Returns the TimeRecord with the given id. 
        /// </summary>
        Task<TimeRecord> GetAsync(Guid id);


        /// <summary>
        /// Returns the Journal with the given TodoId. 
        /// </summary>
        Task<IEnumerable<TimeRecord>> GetAsyncByTodo(Guid todoId);


        /// <summary>
        /// Returns the Journal with the given Date. 
        /// </summary>
        Task<IEnumerable<TimeRecord>> GetAsyncByDate(DateTime redordingDate);
        Task<IEnumerable<TimeRecord>> GetAsyncByDate(DateTime redordingDayStart, DateTime redordingDayEnd);

        /// <summary>
        /// Adds a new TimeRecord if the TimeRecord does not exist, updates the 
        /// existing TimeRecord otherwise.
        /// </summary>
        Task<TimeRecord> UpsertAsync(TimeRecord timeRecord);

        /// <summary>
        /// Deletes a TimeRecord.
        /// </summary>
        Task DeleteAsync(Guid timeRecordId);
    }
}
