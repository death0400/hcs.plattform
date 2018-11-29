using System;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
namespace Hcs.Platform.Flow
{
    public delegate IDependencyInjectionFlowBuilderContext<TNext> DIFlowBuilderHandler<T, TNext>(IDependencyInjectionFlowBuilderContext<T> builder);
    public delegate IFlowBuilderContext<TNext> FlowBuilderHandler<T, TNext>(IFlowBuilderContext<T> builder);
    public class FlowBuilderContext<TStart, TEnd, TOutput> : IFlowBuilderContext<TOutput>
    {
        internal FlowBuilderContext(MergedFlowProcessor<TStart, TEnd> merged, Func<Action<TOutput>, Task> node)
        {
            this.merged = merged;
            this.node = node;
        }
        public void Build()
        {
            if (typeof(TEnd).IsAssignableFrom(typeof(TOutput)))
            {
                merged.FinalGetOutput = (Func<Action<TEnd>, Task>)node;
            }
            else
            {
                throw new Exception($"can not build MergedFlowProcessor {typeof(TEnd).GetFriendlyName()} not assignable form {typeof(TOutput).GetFriendlyName()}");
            }
        }
        public virtual IFlowBuilderContext<TNext> CreateNext<TNext>(Func<Action<TNext>, Task> node)
        {
            return new FlowBuilderContext<TStart, TEnd, TNext>(merged, node);
        }

        private readonly MergedFlowProcessor<TStart, TEnd> merged;
        protected Func<Action<TOutput>, Task> node;
        public IFlowBuilderContext<TNext> Then<TNext>(Func<IFlowProcessor<TOutput, TNext>> processor)
        {
            return CreateNext(GetNextNode(processor));
        }



        public IFlowBuilderContext<TNext> Branch<TNext>(Action<IBranchFlowNodeBuilderContext<TOutput, TNext>> caseConfigurator)
        {
            Func<Action<TNext>, Task> a = GetNextBranch(caseConfigurator);
            return CreateNext(a);
        }
        protected Func<Action<TNext>, Task> GetNextNode<TNext>(Func<IFlowProcessor<TOutput, TNext>> processor)
        {
            return new Func<Action<TNext>, Task>(async (done) =>
            {
                TOutput v = default(TOutput);
                await node(output => v = output);
                var p = processor();
                await p.ProcessAsync(v, done);
            });
        }
        Func<Action<TNext>, Task> GetNextBranch<TNext>(Action<IBranchFlowNodeBuilderContext<TOutput, TNext>> caseConfigurator)
        {
            var context = new BranchFlowNodeBuilderContext<TOutput, TNext>();
            caseConfigurator(context);
            return new Func<Action<TNext>, Task>(async (done) =>
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
        }

        public IFlowBuilderContext<TNext> Do<TNext>(Func<TOutput, TNext> run)
        {
            return Then(() => new DelegateFlowProcessor<TOutput, TNext>(run));
        }

        public IFlowBuilderContext<TOutput> Run(Action<TOutput> run)
        {
            return Then(() => new DelegateFlowProcessor<TOutput, TOutput>(input =>
            {
                run(input);
                return input;
            }));
        }

        public IFlowBuilderContext<TNext> DoAsync<TNext>(Func<TOutput, Task<TNext>> run)
        {
            return Then(() => new AsyncDelegateFlowProcessor<TOutput, TNext>(run));
        }

        public IFlowBuilderContext<TOutput> RunAsync(Func<TOutput, Task> run)
        {
            return Then(() => new AsyncDelegateFlowProcessor<TOutput, TOutput>(async input =>
            {
                await run(input);
                return input;
            }));
        }

        public IFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, TNext> run)
        {
            return Then(() => new DelegateFlowProcessor<TOutput, TNext>(run));
        }

        public IFlowBuilderContext<TOutput> Pipe(Action<TOutput> run)
        {
            return Then(() => new DelegateFlowProcessor<TOutput, TOutput>(input => { run(input); return input; }));
        }

        public IFlowBuilderContext<TNext> Pipe<TNext>(Func<TOutput, Task<TNext>> run)
        {
            return Then(() => new AsyncDelegateFlowProcessor<TOutput, TNext>(run));
        }

        public IFlowBuilderContext<TOutput> Pipe(Func<TOutput, Task> run)
        {
            return Then(() => new AsyncDelegateFlowProcessor<TOutput, TOutput>(async input => { await run(input); return input; }));
        }
    }
}