using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace Pomoductive.Models
{
    public class TimeRecord
    {
        public Guid Id { get; set; } = Guid.NewGuid();
       
        public Guid TodoId { get; set; }
        public DateTime RecordingDay { get; set; } = DateTime.Today;
        public int TaskMin { get; set; }

        public int TotalTaskCount { get; set; } = 0;
        public float Remainder { get; set; } = 0;

        public TimeRecord()
        { }
        public TimeRecord(Guid todoId)
        {
            TodoId = todoId;
        }
    }
}
