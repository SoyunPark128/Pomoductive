using Microsoft.EntityFrameworkCore;
using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Repository.Sql
{
    public class SqlJournalRepository : IJournalRepository
    {
        private readonly PomoductiveContext _db;

        public SqlJournalRepository(PomoductiveContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<Journal>> GetAsync()
        {
            return await _db.Journals
                .AsNoTracking()
                .ToListAsync();
        }

        public async Task<Journal> GetAsync(Guid id)
        {
            return await _db.Journals
                .AsNoTracking()
                .FirstOrDefaultAsync(Journal => Journal.Id == id);
        }

        public async Task<IEnumerable<Journal>> GetAsyncByDate(DateTime redordingDate)
        {
            return await _db.Journals
                .AsNoTracking()
                .Where(j => j.JournalDate == redordingDate)
                .ToListAsync();
        }

        public async Task<Journal> UpsertAsync(Journal journal)
        {
            var current = await _db.Journals.FirstOrDefaultAsync(_journal => _journal.Id == journal.Id);
            if (null == current)
            {
                _db.Journals.Add(journal);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(journal);
            }
            await _db.SaveChangesAsync();
            return journal;
        }

        public async Task DeleteAsync(Guid id)
        {
            var Journal = await _db.Journals.FirstOrDefaultAsync(_timeRecord => _timeRecord.Id == id);
            if (null != Journal)
            {
                _db.Journals.Remove(Journal);
                await _db.SaveChangesAsync();
            }
        }
        
    }
}
