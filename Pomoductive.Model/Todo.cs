using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pomoductive.Models
{
    public class Todo
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get; set; }
        public string Reward { get; set; }

        [NotMapped]
        public bool IsSelected { get; set; }
        public bool IsTerminated { get; set; }
        [NotMapped]
        public bool IsDailyCompleted { get; set; }
        
        public Guid ParentsTodo { get; set; }

        public int DailyCount { get; set; } = 0;
        public int TaskMinutesPerOnePomo { get; set; } = 25;
        public int ShortBreakMinutesPerOnePomo { get; set; } = 5;
        public int LongBreakMinutesPerOnePomo { get; set; } = 15;

        [ForeignKey("TodoId")]
        public ICollection<TimeRecord> TimeRecords { get; set; } = new Collection<TimeRecord>();

        public Todo(string name, string reward = null, Guid parentsTodo = default(Guid))
        {
            Name = name;
            Reward = reward;
            IsSelected = false;
            IsTerminated = false;
            ParentsTodo = parentsTodo;
        }


    }

    public enum Repetitiveness
    {
        Daily,
        OneOff
    }


}
