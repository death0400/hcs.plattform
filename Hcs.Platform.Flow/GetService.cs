using System;
using Hcs.Platform.Flow;
using Microsoft.Extensions.DependencyInjection;
namespace Hcs.Platform.Flow
{
    public class GetService<T, TService> : FlowProcessor<T, IGetServiceOutput<T, TService>> where TService : class
    {
        private readonly TService service;

        public GetService(TService service)
        {
            this.service = service;
        }
        public override IGetServiceOutput<T, TService> Process(T input)
        {
            return new GetServiceOutput<T, TService>(input, service);
        }
    }

}