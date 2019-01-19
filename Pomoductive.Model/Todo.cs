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
        public int DailyCount { get; set; }

        [NotMapped]
        public List<Todo> SubTodos { get; set; } = new List<Todo>();
        public Guid ParentsTodo { get; set; } = Guid.Empty;

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
