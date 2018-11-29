using System;
using System.Threading.Tasks;

namespace Hcs.Platform.Flow
{
    public class AsyncDelegateFlowProcessor<T> : DelegateFlowProcessor<T, T>
    {
        public AsyncDelegateFlowProcessor(Func<T, T> process) : base(process)
        {
        }
    }
    public class AsyncDelegateFlowProcessor<TIn, TOut> : AsyncFlowProcessor<TIn, TOut>
    {
        private readonly Func<TIn, Task<TOut>> process;

        public AsyncDelegateFlowProcessor(Func<TIn, Task<TOut>> process)
        {
            this.process = process;
        }

        public override async Task ProcessAsync(TIn input, Action<TOut> done)
        {
            done(await process(input));
        }
    }
}