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
        /// Returns all customers. 
        /// </summary>
        Task<IEnumerable<TimeRecord>> GetAsync();

        /// <summary>
        /// Returns the customer with the given id. 
        /// </summary>
        Task<TimeRecord> GetAsync(Guid id);

        Task<IEnumerable<TimeRecord>> GetAsyncByTodo(Guid todoId);

        Task<IEnumerable<TimeRecord>> GetAsyncByDate(DateTime redordingDay);

        /// <summary>
        /// Adds a new customer if the customer does not exist, updates the 
        /// existing customer otherwise.
        /// </summary>
        Task<TimeRecord> UpsertAsync(TimeRecord customer);

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        Task DeleteAsync(Guid customerId);
    }
}
