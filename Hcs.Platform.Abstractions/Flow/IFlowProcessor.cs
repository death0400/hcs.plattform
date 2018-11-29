using System.Linq;
using System.Threading.Tasks;
using System.Threading;
using System;
using System.Collections.Concurrent;
namespace Hcs.Platform.Flow
{
    public interface IFlowProcessor<in TInput, out TOutput>
    {
        Task ProcessAsync(TInput input, Action<TOutput> done);
    }
    
}