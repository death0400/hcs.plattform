using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform.Flow
{
    public class DependencyInjectionFlowBuilderContext<TStart, TEnd, TOutput> : FlowBuilderContext<TStart, TEnd, TOutput>, IDependencyInjectionFlowBuilderContext<TOutput>
    {
        private readonly MergedFlowProcessor<TStart, TEnd> merged;

        public IServiceProvider ServiceProvider { get; }

        internal DependencyInjectionFlowBuilderContext(MergedFlowProcessor<TStart, TEnd> merged, IServiceProvider serviceProvider, Func<Action<TOutput>, Task> node) : base(merged, node)
        {
            this.merged = merged;
            this.ServiceProvider = serviceProvider;
        }
        public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext, TFlowProcessor>() where TFlowProcessor : IFlowProcessor<TOutput, TNext>
        {
            return Then<TNext>(() => ServiceProvider.GetRequiredService<TFlowProcessor>());
        }
        public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Type processorType)
        {
            if (typeof(IFlowProcessor<TOutput, TNext>).IsAssignableFrom(processorType))
            {
                return Then<TNext>(() => (IFlowProcessor<TOutput, TNext>)ServiceProvider.GetRequiredService(processorType));
            }
            else
            {
                throw new ArgumentException($"{processorType.GetFriendlyName()} is not {typeof(IFlowProcessor<TOutput, TNext>).GetFriendlyName()}");
            }
        }
        new public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IFlowProcessor<TOutput, TNext>> processor)
        {
            Func<Action<TNext>, Task> a = GetNextNode(processor);
            return new DependencyInjectionFlowBuilderContext<TStart, TEnd, TNext>(merged, ServiceProvider, a);
        }

        public IDependencyInjectionFlowBuilderContext<TNext> Branch<TNext>(Action<IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext>> caseConfigurator)
        {
            Func<Action<TNext>, Task> a = GetNextBranch(caseConfigurator);
            return new DependencyInjectionFlowBuilderContext<TStart, TEnd, TNext>(merged, ServiceProvider, a);
        }
        Func<Action<TNext>, Task> GetNextBranch<TNext>(Action<IDependencyInjectionBranchFlowNodeBuilderContext<TOutput, TNext>> caseConfigurator)
        {
            var context = new DIBranchFlowNodeBuilderContext<TOutput, TNext>(ServiceProvider);
            caseConfigurator(context);
            var a = new Func<Action<TNext>, Task>(async (done) =>
            {
                TOutput v = default(TOutput);
                await node(output => v = output);
                IFlowProcessor<TOutput, TNext> processor = null;
                foreach (var c in context.Cases)
                {
                    if (c.Key(v))
                    {
                        processor = c.Value();
                        break;
                    }
                }
                if (processor == null)
                {
                    throw new Exception("no process match with output");
                }
                else
                {
                    await processor.ProcessAsync(v, done);
                }
            });
            return a;
        }
        public IDependencyInjectionFlowBuilderContext<IGetServiceOutput<TOutput, TService>> GetService<TService>() where TService : class
        {
            return Then<IGetServiceOutput<TOutput, TService>, GetService<TOutput, TService>>();
        }

        public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IServiceProvider, IFlowProcessor<TOutput, TNext>> processorActivator)
        {
            return Then<TNext>(() => processorActivator(ServiceProvider));
        }

        public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IServiceProvider, TOutput, TNext> delegateWithServiceProvider)
        {
            return Then<TNext>(() => new DelegateFlowProcessor<TOutput, TNext>(input => delegateWithServiceProvider(ServiceProvider, input)));
        }

        public IDependencyInjectionFlowBuilderContext<TNext> Then<TNext>(Func<IServiceProvider, TOutput, Task<TNext>> delegateWithServiceProvider)
        {
            return Then<TNext>(() => new AsyncDelegateFlowProcessor<TOutput, TNext>(input => delegateWithServiceProvider(ServiceProvider, input)));
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1>(Action<TService1, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2>(Action<TService1, TService2, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3>(Action<TService1, TService2, TService3, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4>(Action<TService1, TService2, TService3, TService4, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5>(Action<TService1, TService2, TService3, TService4, TService5, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Action<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput> run)
        {
            return Then((sp, input) =>
            {
                sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>>().InvokeMethod(input, run);
                return input;
            });
        }
        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1>(Func<TService1, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2>(Func<TService1, TService2, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>>().InvokeMethod(input, run);
                return input;
            });
        }


        public IDependencyInjectionFlowBuilderContext<TOutput> Pipe<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, Task> run)
        {
            return Then(async (sp, input) =>
            {
                await sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>>().InvokeMethod(input, run);
                return input;
            });
        }




        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1>(Func<TService1, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2>(Func<TService1, TService2, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, TNext> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>>().InvokeMethod(input, run));
        }



        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1>(Func<TService1, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2>(Func<TService1, TService2, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3>(Func<TService1, TService2, TService3, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4>(Func<TService1, TService2, TService3, TService4, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5>(Func<TService1, TService2, TService3, TService4, TService5, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8>>().InvokeMethod(input, run));
        }


        public IDependencyInjectionFlowBuilderContext<TNext> Pipe<TNext, TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>(Func<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9, TOutput, Task<TNext>> run)
        {
            return Then((sp, input) => sp.GetRequiredService<ServiceBundle<TService1, TService2, TService3, TService4, TService5, TService6, TService7, TService8, TService9>>().InvokeMethod(input, run));
        }




        IDependencyInjectionFlowBuilderContext<TNext> IDependencyInjectionFlowBuilderContext<TOutput>.Pipe<TNext>(Func<TOutput, TNext> run)
        {
            return Then<TNext>(() => new DelegateFlowProcessor<TOutput, TNext>(run));
        }

        IDependencyInjectionFlowBuilderContext<TOutput> IDependencyInjectionFlowBuilderContext<TOutput>.Pipe(Action<TOutput> run)
        {
            return Then<TOutput>(() => new DelegateFlowProcessor<TOutput, TOutput>(input => { run(input); return input; }));
        }

        IDependencyInjectionFlowBuilderContext<TNext> IDependencyInjectionFlowBuilderContext<TOutput>.Pipe<TNext>(Func<TOutput, Task<TNext>> run)
        {
            return Then<TNext>(() => new AsyncDelegateFlowProcessor<TOutput, TNext>(run));
        }

        IDependencyInjectionFlowBuilderContext<TOutput> IDependencyInjectionFlowBuilderContext<TOutput>.Pipe(Func<TOutput, Task> run)
        {
            return Then<TOutput>(() => new DelegateFlowProcessor<TOutput, TOutput>(input => { run(input); return input; }));
        }
    }

}
