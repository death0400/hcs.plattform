using System;
using System.Threading;
using Xunit;
using Hcs.Platform.Flow;
namespace Hcs.Platform.Flow.Tests
{
    class Plus : FlowProcessor<int, int>
    {
        readonly int n;
        public Plus(int n)
        {
            this.n = n;
        }
        public override int Process(int input) => input + n;
    }

    class Multiply : FlowProcessor<decimal, decimal>
    {
        readonly int n;
        public Multiply(int n)
        {
            this.n = n;
        }

        public override decimal Process(decimal input) => input * n;
    }
    public class ProcessorTest
    {
        [Fact]
        public async void TestSimple()
        {
            var processor = new MergedFlowProcessor<int, string>(builder =>
                builder
                    .Then(new Plus(2))
                    .Pipe(x => (decimal)x)
                    .Then(new Multiply(2))
                    .Pipe(x => x.ToString())
                    );
            Assert.Equal("204", await processor.Run(100));

        }
        [Fact]
        public async void TestBranch()
        {
            var processor = new MergedFlowProcessor<int, int>(builder => builder.Then(new Plus(2)).Branch<int>(c =>
                 {
                     c.AddCase(x => x > 100, new Plus(100));
                     c.AddCase(x => x <= 100, new Plus(-100));
                 }));
            var ans1 = await processor.Run(99);
            var ans2 = await processor.Run(98);
            Assert.True(ans1 == 201 && ans2 == 0);

        }
    }
}
