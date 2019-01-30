using Microsoft.EntityFrameworkCore;
using Pomoductive.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Repository.Sql
{
    public class SqlTimeRecordRepository : ITimeRecordRepository
    { 
        private readonly PomoductiveContext _db;

        public SqlTimeRecordRepository(PomoductiveContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<TimeRecord>> GetAsync()
        {
            return await _db.TimeRecords
                .AsNoTracking()
                .OrderBy(tr => tr.RedordingDate)
                .ToListAsync();
        }

        public async Task<TimeRecord> GetAsync(Guid id)
        {
            return await _db.TimeRecords
                .AsNoTracking()
                .OrderBy(tr => tr.RedordingDate)
                .FirstOrDefaultAsync(timeRecord => timeRecord.Id == id);
        }


        public async Task<IEnumerable<TimeRecord>> GetAsyncByTodo(Guid todoId)
        {
            return await _db.TimeRecords
                .AsNoTracking()
                .Where(tr => tr.TodoId == todoId)
                .OrderBy(tr => tr.RedordingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimeRecord>> GetAsyncByDate(DateTime redordingDate)
        {
            return await _db.TimeRecords
                .AsNoTracking()
                .Where(tr => tr.RedordingDate == redordingDate)
                .OrderBy(tr => tr.RedordingDate)
                .ToListAsync();
        }

        public async Task<IEnumerable<TimeRecord>> GetAsyncByDate(DateTime redordingDayStart, DateTime redordingDayEnd)
        {
            return await _db.TimeRecords
                .AsNoTracking()
                .Where(tr => tr.RedordingDate > redordingDayStart && tr.RedordingDate <= redordingDayEnd)
                .OrderBy(tr => tr.RedordingDate)
                .ToListAsync();
        }

        public async Task<TimeRecord> UpsertAsync(TimeRecord timeRecord)
        {
            var current = await _db.TimeRecords.FirstOrDefaultAsync(_timeRescord => _timeRescord.Id == timeRecord.Id);
            if (null == current)
            {
                _db.TimeRecords.Add(timeRecord);
            }
            else
            {
                _db.Entry(current).CurrentValues.SetValues(timeRecord);
            }
            await _db.SaveChangesAsync();
            return timeRecord;
        }
        public async Task DeleteAsync(Guid id)
        {
            var timeRecord = await _db.TimeRecords.FirstOrDefaultAsync(_timeRecord => _timeRecord.Id == id);
            if (null != timeRecord)
            {
                _db.TimeRecords.Remove(timeRecord);
                await _db.SaveChangesAsync();
            }
        }
    }
}
