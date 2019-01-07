using Xunit;
using System;
using Pomoductive;
using Pomoductive.Model;

namespace UnitTest
{
    
    public class UnitTest1
    {
        [Fact]
        public void makeTask()
        {
            Job testTask = new Category("test Task");

            Assert.Equal("test Task", testTask.name);
        }

        [Fact]
        public void TimeCountTest()
        {
            //arrange
            Job testTask = new Category("time count test Task");

            //act
            testTask.TimeCountStart();
            System.Threading.Tasks.Task.Delay(2000).Wait();
            testTask.TimeCountStop();

            TimeSpan assumedTime1 = new TimeSpan(0, 0, 2);
            TimeSpan assumedTime2 = new TimeSpan(0, 0, 3);

            //assert
            Assert.InRange(testTask.GetTotalTime(), assumedTime1, assumedTime2);

        }

        [Fact]
        public void MultipleTimeCountTest()
        {
            Job testTask = new Category("multiple time count test Task");

            //act
            testTask.TimeCountStart();
            System.Threading.Tasks.Task.Delay(2000).Wait();
            testTask.TimeCountStop();

            testTask.TimeCountStart();
            System.Threading.Tasks.Task.Delay(3000).Wait();
            testTask.TimeCountStop();

            TimeSpan assumedTime1 = new TimeSpan(0, 0, 5);
            TimeSpan assumedTime2 = new TimeSpan(0, 0, 6);

            //assert
            Assert.InRange(testTask.GetTotalTime(), assumedTime1, assumedTime2);
        }

    }
}
