using System;
using System.Collections.Generic;
using System.Text;
using Pomoductive.Models;
using Microsoft.EntityFrameworkCore;

namespace Pomoductive.Repository.Sql
{
    /// <summary>
    /// Entity Framework Core DbContext for Pomoductive.
    /// </summary>
    public class PomoductiveContext : DbContext
    {
        /// <summary>
        /// Creates a new Pomoductive DbContext.
        /// </summary>
        public PomoductiveContext(DbContextOptions<PomoductiveContext> options) : base(options)
        { }

        /// <summary>
        /// Gets the todos DbSet.
        /// </summary>
        public DbSet<Todo> Todos { get; set; }

        /// <summary>
        /// Gets the timeRecord DbSet.
        /// </summary>
        public DbSet<TimeRecord> TimeRecords { get; set; }

        /// <summary>
        /// Gets the Journals DbSet.
        /// </summary>
        public DbSet<Journal> Journals { get; set; }
    }
}
