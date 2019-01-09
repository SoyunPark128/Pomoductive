using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pomoductive.Models
{
    public class Todo
    {
        public string Name { get; set; }
        public string Reward { get; set; }

        public bool IsSelected { get; set; }
        public bool IsTerminated { get; set; }
        public bool IsDailyCompleted { get; set; }
        public int DailyCount { get; set; }

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
