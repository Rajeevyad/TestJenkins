using System;
using Xunit;

namespace TestJenkinsTest
{
    public class UnitTest1
    {
        [Fact]
        public void Test1()
        {
            var x = 6;
            Assert.Equal<int>(6, x);
        }

        [Fact]
        public void Test2()
        {
            var x = 6;
            Assert.Equal<int>(6, x);
        }


        [Fact]
        public void Test3()
        {
            var x = 6;
            Assert.Equal<int>(6, x);
        }
    }
}
