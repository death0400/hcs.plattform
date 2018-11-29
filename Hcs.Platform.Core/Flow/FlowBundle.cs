using System;
using Hcs.Platform.Core.PlatformModule;
using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Flow
{
    internal class FlowBundle : IFlowBundle
    {
        private readonly IServiceProvider serviceProvider;

        public FlowBundle(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        public IFlowProcessor<HttpRequest, IActionResult> GetFlowProcessor(string key)
        {
            var token = EntityApiContextContainer.Get(key);
            var p = new MergedFlowProcessor<HttpRequest, IActionResult>();
            var builder = p.Start(serviceProvider);
            token.BuildFlow(builder).Build();

            return p;
        }
    }
}