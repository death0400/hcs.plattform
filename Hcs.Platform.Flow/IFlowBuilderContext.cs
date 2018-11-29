using System;
using System.Threading.Tasks;

namespace Hcs.Platform.Flow
{
    public interface IFlowBuilderContext<out TOutput>
    {
        void Build();
        IFlowBuilderContext<TNOutput> Then<TNOutput>(Func<IFlowProcessor<TOutput, TNOutput>> processor);
        IFlowBuilderContext<TNext> Branch<TNext>(Action<IBranchFlowNodeBuilderContext<TOutput, TNext>> options);
        IFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, TNext> run);
        IFlowBuilderContext<TOutput> Pipe(Action<TOutput> run);

        IFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, Task<TNext>> run);
        IFlowBuilderContext<TOutput> Pipe(Func<TOutput, Task> run);
    }
    public static class IFlowBuilderContextExtensions
    {
        public static IFlowBuilderContext<TNOutput> Then<TOutput, TNOutput>(this IFlowBuilderContext<TOutput> builder, IFlowProcessor<TOutput, TNOutput> processor)
        {
            return builder.Then(() => processor);
        }

        public static IDependencyInjectionFlowBuilderContext<TNOutput> Then<TOutput, TNOutput>(this IDependencyInjectionFlowBuilderContext<TOutput> builder, IFlowProcessor<TOutput, TNOutput> processor)
        {
            return builder.Then(() => processor);
        }
    }
}