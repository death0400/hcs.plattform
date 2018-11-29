using System;
using System.Collections.Generic;
namespace Hcs.Platform.Flow
{
    internal class BranchFlowNodeBuilderContext<TOutput, TNext> : IBranchFlowNodeBuilderContext<TOutput, TNext>
    {
        public IDictionary<Func<TOutput, bool>, Func<IFlowProcessor<TOutput, TNext>>> Cases { get; } = new Dictionary<Func<TOutput, bool>, Func<IFlowProcessor<TOutput, TNext>>>();


        public void AddCase(Func<TOutput, bool> condition, Func<IFlowProcessor<TOutput, TNext>> activator)
        {
            Cases.Add(condition, activator);
        }
    }
    internal class DIBranchFlowNodeBuilderContext<TOutput, TNext> : IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext>
    {
        public IDictionary<Func<TOutput, bool>, Func<IFlowProcessor<TOutput, TNext>>> Cases { get; } = new Dictionary<Func<TOutput, bool>, Func<IFlowProcessor<TOutput, TNext>>>();

        public IServiceProvider ServiceProvider { get; }
        public DIBranchFlowNodeBuilderContext(IServiceProvider serviceProvider)
        {
            ServiceProvider = serviceProvider;
        }
        public void AddCase(Func<TOutput, bool> condition, Func<IFlowProcessor<TOutput, TNext>> activator)
        {
            Cases.Add(condition, activator);
        }
    }
}