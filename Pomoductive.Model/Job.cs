using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Pomoductive.Model
{
    public class Job
    {
        public string Name { get; set; }
        public string Reward { get; set; }

        public bool IsSelected { get; set; }
        public bool IsFinished { get; set; }

        public Job(string name, string reward = null)
        {
            Name = name;
            Reward = reward;
            IsSelected = false;
            IsFinished = false;
        }

        public void MarkFinished()
        {
            IsFinished = true;
        }
    }


    
}
