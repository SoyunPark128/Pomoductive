using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Model
{
    public class Todo : Job
    {
        public Todo(string name, string reward = null):base(name, reward)
        {
        }
        
    }
}
