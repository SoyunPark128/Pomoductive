using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Pomoductive.Models;

namespace Pomoductive.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the app backend using 
    /// SQL via Entity Framework Core 2.2. 
    /// </summary>
    public class SqlPomoductiveRepository : IPomoductiveRepository
    {
        private readonly DbContextOptions<PomoductiveContext> _dbOptions;

        public SqlPomoductiveRepository(DbContextOptionsBuilder<PomoductiveContext>  dbOptionsBuilder)
        {
            _dbOptions = dbOptionsBuilder.Options;
            using (var db = new PomoductiveContext(_dbOptions))
            {
                db.Database.EnsureCreated();
            }
        }



        public ITodoRepository Todos => new SqlTodoRepository(new PomoductiveContext(_dbOptions));

        public ITimeRecordRepository TimeRecords => new SqlTimeRecordRepository(new PomoductiveContext(_dbOptions));

        public IJournalRepository Journals => new SqlJournalRepository(new PomoductiveContext(_dbOptions));

    }
}
