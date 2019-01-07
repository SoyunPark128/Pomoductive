using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Pomoductive.Model
{
    public class DailyCategory : Category, IDaily
    {
        private DailyTodo[] dailySubTasks;

        public DailyCategory(string name, string reward = null) :base(name, reward)
        {
        }
        public void RetrieveDailyTask()
        {
            IsFinished = false;
        }
    }


}
