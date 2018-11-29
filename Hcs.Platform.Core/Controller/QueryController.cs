using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using System;
using Hcs.Platform.Flow;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace Hcs.Platform.Core.Controller
{
    [Authorize]
    [GenericControllerNameConvention]
    [Route("api/entity/[controller]")]
    public class QueryController<TEntity> : PlatformControllerBase where TEntity : class
    {
        public QueryController(IFlowBundle flowBundle, IServiceProvider serviceProvider, IAuthorizationService authorizationService, IHttpContextAccessor context) : base(flowBundle, serviceProvider, authorizationService, context)
        {
        }
        [HttpGet]
        public async Task<IActionResult> Query(ODataQueryOptions<TEntity> oDataQueryOptions)
        {
            return await AuthorizationThen(nameof(Query), async () =>
                   {
                       var context = ServiceProvider.GetRequiredService<OdataQueryRequestContext<TEntity>>();
                       context.ODataQueryOptions = oDataQueryOptions;
                       var processor = FlowBundle.GetFlowProcessor(FlowKey + "." + nameof(Query));
                       var output = await processor.Run(Request);
                       return output;
                   });
        }
    }
}