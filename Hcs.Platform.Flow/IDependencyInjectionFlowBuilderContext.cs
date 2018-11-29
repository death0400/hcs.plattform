using System;
using System.Threading.Tasks;

namespace Hcs.Platform.Flow
{
    public interface IDependencyInjectionFlowBuilderContext<out TOutput> : IFlowBuilderContext<TOutput>
    {
        IDependencyInjectionFlowBuilderContext<TNext> Then<TNext, TFlowProcessor>() where TFlowProcessor : IFlowProcessor<TOutput, TNext>;
        IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Type processorType);
        IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IServiceProvider, IFlowProcessor<TOutput, TNext>> processorActivator);

        new IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IFlowProcessor<TOutput, TNext>> processor);
        IDependencyInjectionFlowBuilderContext<IGetServiceOutput<TOutput, TService>> GetService<TService>() where TService : class;
        IDependencyInjectionFlowBuilderContext<TNext> Branch<TNext>(Action<IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext>> options);
        new IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, TNext> run);
        new IDependencyInjectionFlowBuilderContext<TOutput> Pipe(Action<TOutput> run);

        new IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, Task<TNext>> run);
        new IDependencyInjectionFlowBuilderContext<TOutput> Pipe(Func<TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1>(Func<TService1, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2>(Func<TService1, TService2, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, TNext> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1>(Action<TService1, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2>(Action<TService1, TService2, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3>(Action<TService1, TService2, TService3, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4>(Action<TService1, TService2, TService3, TService4, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5>(Action<TService1, TService2, TService3, TService4, TService5, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1>(Func<TService1, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2>(Func<TService1, TService2, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, Task<TNext>> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1>(Func<TService1, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2>(Func<TService1, TService2, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, Task> run);
        IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, Task> run);

    }
}