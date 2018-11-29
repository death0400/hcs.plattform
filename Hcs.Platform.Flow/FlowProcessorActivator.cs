using System;
namespace Hcs.Platform.Flow
{
    public delegate IFlowProcessor<TInput, TOutput> FlowProcessorActivator<in TInput, TOutput>(IServiceProvider serviceProvider);
}