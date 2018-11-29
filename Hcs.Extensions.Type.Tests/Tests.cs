using System;
using Xunit;
namespace Hcs.Extensions.Type.Tests
{
    public class Tests
    {
        [Fact]
        public void TestType()
        {
            var name = typeof(string).GetFriendlyName();
            Assert.Equal("String", name);
        }
        [Fact]
        public void TestGenricType()
        {
            var name = typeof(Action<int, string>).GetFriendlyName();
            Assert.Equal("Action<Int32,String>", name);
        }
        [Fact]
        public void TestGenricTypeFull()
        {
            var name = typeof(Action<int, string>).GetFriendlyFullName();
            Assert.Equal("System.Action<System.Int32,System.String>", name);
        }
        [Fact]
        public void TestNestedGenricType()
        {
            var name = typeof(Action<int, Action<int, Action<int, string>>>).GetFriendlyName();
            Assert.Equal("Action<Int32,Action<Int32,Action<Int32,String>>>", name);
        }
        [Fact]
        public void TestNestedGenricTypeFull()
        {
            var name = typeof(Action<int, Action<int, Action<int, string>>>).GetFriendlyFullName();
            Assert.Equal("System.Action<System.Int32,System.Action<System.Int32,System.Action<System.Int32,System.String>>>", name);
        }
    }
}
