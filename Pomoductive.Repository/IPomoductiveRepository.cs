﻿using System;
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

        /// <summary>
        /// Returns the TimeRecords repository.
        /// </summary>
        ITimeRecordRepository TimeRecords { get; }


        /// <summary>
        /// Returns the Journals repository.
        /// </summary>
        IJournalRepository Journals { get; }

    }
}
