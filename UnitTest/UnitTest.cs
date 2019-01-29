using Xunit;
using System;
using Pomoductive.Models;
using Pomoductive;

namespace UnitTest
{

    public class UnitTest1
    {
        [Fact]
        public void makeTask()
        {
            Todo testTask = new Todo("test Task");

            Assert.Equal("test Task", testTask.Name);
        }


        [Fact]
        public void TimeCountTest()
        {
            //arrange
            Todo testTask = new Todo("time count test Task");

            //act
            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountReStart();
            System.Threading.Tasks.Task.Delay(2000).Wait();
            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountStop();


            TimeSpan assumedTime1 = new TimeSpan(0, 0, 2);
            TimeSpan assumedTime2 = new TimeSpan(0, 0, 3);

            //assert
            Assert.InRange(Pomoductive.App.AppViewModel.AppStopwatchViewModel.GetTotalTime(), assumedTime1, assumedTime2);

        }

        [Fact]
        public void MultipleTimeCountTest()
        {
            Todo testTask = new Todo("multiple time count test Task");

            //act
            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountStart();
            System.Threading.Tasks.Task.Delay(2000).Wait();
            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountStop();

            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountReStart();
            System.Threading.Tasks.Task.Delay(3000).Wait();
            Pomoductive.App.AppViewModel.AppStopwatchViewModel.TimeCountStop();

            TimeSpan assumedTime1 = new TimeSpan(0, 0, 5);
            TimeSpan assumedTime2 = new TimeSpan(0, 0, 6);

            //assert
            Assert.InRange(Pomoductive.App.AppViewModel.AppStopwatchViewModel.GetTotalTime(), assumedTime1, assumedTime2);
        }

    }
}
