using Microsoft.EntityFrameworkCore;
using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Repository.Sql
{
    /// <summary>
    /// Contains methods for interacting with the Todos backend using 
    /// SQL via Entity Framework Core 2.2.
    /// </summary>
    public class SqlTodoRepository
    {
        private readonly PomoductiveContext _db;

        public SqlTodoRepository(PomoductiveContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Todo>> GetAsync()
        {
            return await _db.Todos
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Todo> GetAsync(Guid id)
        {
            return await _db.Todos
                .AsNoTracking()
                .FirstOrDefaultAsync(todo => todo.Id == id);
        }

        public async Task<IEnumerable<Todo>> GetAsync(string value)
        {
            string[] parameters = value.Split(' ');
            return await _db.Todos
                .Where(todo =>
                    parameters.Any(parameter => todo.Name.StartsWith(parameter) ))
                .OrderByDescending(todo =>
                    parameters.Count(parameter => todo.Name.StartsWith(parameter)))
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Todo> UpsertAsync(Todo todo)
        {
            var current = await _db.Todos.FirstOrDefaultAsync(_todo => _todo.Id == todo.Id);
            if (null == current)
            {
                _db.Todos.Add(todo);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(todo);
            }
            await _db.SaveChangesAsync();
            return todo;
        }
        public async Task DeleteAsync(Guid id)
        {
            var todo = await _db.Todos.FirstOrDefaultAsync(_todo => _todo.Id == id);
            if (null != todo)
            {
                _db.Todos.Remove(todo);
                await _db.SaveChangesAsync();
            }
        }
    }
}
