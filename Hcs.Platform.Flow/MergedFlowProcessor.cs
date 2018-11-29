using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;

namespace Hcs.Platform.Flow
{
    public class StartFlowProcessor<TInpit> : FlowProcessor<TInpit, TInpit>
    {
        public override TInpit Process(TInpit input) => input;
    }
    public class MergedFlowProcessor<TInput, TOutput> : IFlowProcessor<TInput, TOutput>
    {
        public MergedFlowProcessor()
        {

        }
        public MergedFlowProcessor(IServiceProvider serviceProvider, DIFlowBuilderHandler<TInput, TOutput> builder)
        {
            builder(Start(serviceProvider)).Build();
        }
        public MergedFlowProcessor(FlowBuilderHandler<TInput, TOutput> builder)
        {
            builder(Start()).Build();
        }

        public TInput Input { get; set; }

        public string Name { get; set; }

        public Func<Action<TOutput>, Task> FinalGetOutput { get; set; }
        public IDependencyInjectionFlowBuilderContext<TInput> Start(IServiceProvider serviceProvider)
        {
            return new DependencyInjectionFlowBuilderContext<TInput, TOutput, TInput>(this, serviceProvider, async done =>
             {
                 await Task.CompletedTask;
                 done(Input);
             });
        }
        public IFlowBuilderContext<TInput> Start()
        {
            return new FlowBuilderContext<TInput, TOutput, TInput>(this, async done =>
             {
                 await Task.CompletedTask;
                 done(Input);
             });
        }
        public async Task ProcessAsync(TInput input, Action<TOutput> done)
        {
            Input = input;
            await FinalGetOutput(done);
        }
    }
}