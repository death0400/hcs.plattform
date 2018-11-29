using System.Threading.Tasks;
namespace Hcs.Platform.Flow
{
    public static class IFlowProcessorExtensions
    {
        public static async Task<TOutput> Run<TInput, TOutput>(this IFlowProcessor<TInput, TOutput> processor, TInput input)
        {
            TOutput output = default(TOutput);
            await processor.ProcessAsync(input, o => output = o);
            return output;
        }
    }
}