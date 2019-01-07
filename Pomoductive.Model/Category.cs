using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Model
{
    public class Category:Job
    {
        public Todo[] todos;

        public Category(string name, string reward = null) : base(name, reward)
        {
        }
    }
}
