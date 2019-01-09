using System;
using System.Collections.Generic;
using System.Text;
using System.Diagnostics;

namespace Pomoductive.Models
{
    public class StopWatchModel
    {
        public Stopwatch Stopwatch { get; set; }
        public TimeSpan TotalTime { get; set; }

        private static readonly StopWatchModel instance = new StopWatchModel();

        static StopWatchModel()
        { }

        private StopWatchModel()
        {
            Stopwatch = new Stopwatch();
            TotalTime = new TimeSpan();
        }

        public static StopWatchModel Instance
        {
            get
            {
                return instance;
            }
        }


    }
}
