using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace Hcs.Platform.Flow
{
    public abstract class FlowProcessor<TInput, TOutput> : AsyncFlowProcessor<TInput, TOutput>
    {
        public override async Task ProcessAsync(TInput input, Action<TOutput> done)
        {
            done(Process(input));
            await Task.CompletedTask;
        }
        public abstract TOutput Process(TInput input);
    }
}