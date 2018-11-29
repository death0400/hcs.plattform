using System;
namespace Hcs.Platform.Flow
{
    public interface IBranchFlowNodeBuilderContext<out TOutput, TNext>
    {
        void AddCase(Func<TOutput, bool> condition, Func<IFlowProcessor<TOutput, TNext>> activator);
    }
    public interface IDependencyInjectionBranchFlowNodeBuilderContext<out TOutput, TNext> : IBranchFlowNodeBuilderContext<TOutput, TNext>
    {
        IServiceProvider ServiceProvider { get; }
    }
    public static class IBranchFlowNodeBuilderContextExtensions
    {
        public static IBranchFlowNodeBuilderContext<TOutput, TNext> AddCase<TOutput, TNext>(this IBranchFlowNodeBuilderContext<TOutput, TNext> builder, Func<TOutput, bool> condition, IFlowProcessor<TOutput, TNext> processor)
        {
            builder.AddCase(condition, () => processor);
            return builder;
        }
        public static IBranchFlowNodeBuilderContext<TOutput, TNext> AddCase<TOutput, TNext>(this IBranchFlowNodeBuilderContext<TOutput, TNext> builder, Func<TOutput, bool> condition, FlowBuilderHandler<TOutput, TNext> processor)
        {
            builder.AddCase(condition, () => new MergedFlowProcessor<TOutput, TNext>(processor));
            return builder;
        }
    }
    public static class IDependencyInjectionBranchFlowNodeBuilderContextContextExtensions
    {
        public static IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext> AddCase<TOutput, TNext>(this IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext> builder, Func<TOutput, bool> condition, IFlowProcessor<TOutput, TNext> processor)
        {
            builder.AddCase(condition, () => processor);
            return builder;
        }
        public static IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext> AddCase<TOutput, TNext>(this IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext> builder, Func<TOutput, bool> condition, DIFlowBuilderHandler<TOutput, TNext> processor)
        {
            builder.AddCase(condition, () => new MergedFlowProcessor<TOutput, TNext>(builder.ServiceProvider, processor));
            return builder;
        }
    }
}