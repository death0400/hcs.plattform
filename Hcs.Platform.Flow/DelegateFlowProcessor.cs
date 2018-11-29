using System;

namespace Hcs.Platform.Flow
{
    public class DelegateFlowProcessor<TIn, TOut> : FlowProcessor<TIn, TOut>
    {
        private readonly Func<TIn, TOut> process;

        public DelegateFlowProcessor(Func<TIn, TOut> process)
        {
            this.process = process;
        }
        public override TOut Process(TIn input) => process(input);
    }
    public class DelegateFlowProcessor<T> : DelegateFlowProcessor<T, T>
    {
        public DelegateFlowProcessor(Func<T, T> process) : base(process)
        {
        }
    }
}