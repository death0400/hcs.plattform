using System;
using System.Threading.Tasks;

namespace Hcs.Platform.Flow
{
    public abstract class AsyncFlowProcessor<TInput, TOutput> : IFlowProcessor<TInput, TOutput>
    {
        public string Name { get; set; }
        public abstract Task ProcessAsync(TInput input, Action<TOutput> done);

    }


}