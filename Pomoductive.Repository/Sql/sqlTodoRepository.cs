using System;
using System.Collections.Generic;
using System.Text;

namespace Pomoductive.Repository.Sql
{
    public class SqlTodoRepository
    {
        private readonly PomoductiveContext _db;

        public SqlTodoRepository(PomoductiveContext db)
        {
            _db = db;
        }
    }
}
