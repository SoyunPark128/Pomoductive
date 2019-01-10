using System;
using System.Collections.Generic;
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
        public List<Todo> SubTodos { get; set; }
        
        public Todo(string name, string reward = null)
        {
            Name = name;
            Reward = reward;
            IsSelected = false;
            IsTerminated = false;
        }


    }

    public enum Repetitiveness
    {
        Daily,
        OneOff
    }

    public enum Subdivided
    {
        Category,
        SubTodo
    }



}
