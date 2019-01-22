using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Pomoductive.Models;

namespace Pomoductive.Repository
{
    public interface ITodoRepository
    {
        /// <summary>
        /// Returns all customers. 
        /// </summary>
        Task<IEnumerable<Todo>> GetAsync();

        /// <summary>
        /// Returns all customers with a data field matching the start of the given string. 
        /// </summary>
        Task<IEnumerable<Todo>> GetAsync(string search);

        /// <summary>
        /// Returns the customer with the given id. 
        /// </summary>
        Task<Todo> GetAsync(Guid id);

        Task<IEnumerable<Todo>> GetForParentsTodoAsync();

        Task<IEnumerable<Todo>> GetForSubTodoAsync();

        /// <summary>
        /// Adds a new customer if the customer does not exist, updates the 
        /// existing customer otherwise.
        /// </summary>
        Task<Todo> UpsertAsync(Todo customer);

        /// <summary>
        /// Deletes a customer.
        /// </summary>
        Task DeleteAsync(Guid customerId);
    }
}
