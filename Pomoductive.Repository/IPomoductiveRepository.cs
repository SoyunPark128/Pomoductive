using System;
using System.Collections.Generic;
using System.Text;
using Pomoductive.Models;

namespace Pomoductive.Repository
{
    /// <summary>
    /// Defines methods for interacting with the app backend.
    /// </summary>
    public interface IPomoductiveRepository
    {
        /// <summary>
        /// Returns the Todos repository.
        /// </summary>
        ITodoRepository Todos { get; }
    }
}
