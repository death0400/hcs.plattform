using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System.Transactions;
using Microsoft.Extensions.DependencyInjection;
using System;
using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.AspNetCore.Http;

namespace Hcs.Platform.Core.Controller
{
    [Authorize]
    [GenericControllerNameConvention]
    [Route("api/entity/[controller]")]
    public class EntityController<TKey, TEntity> : PlatformControllerBase where TEntity : class
    {
        public EntityController(IFlowBundle flowBundle, IServiceProvider serviceProvider, IAuthorizationService authorizationService, IHttpContextAccessor context) : base(flowBundle, serviceProvider, authorizationService, context)
        {
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(TKey id)
        {
            return await AuthorizationThen(EntityControllerMethods.Delete, async () =>
            {
                using (var scoep = GetScoep())
                {
                    var context = ServiceProvider.GetRequiredService<KeyRequestContext<TKey>>();
                    context.TransactionScope = scoep;
                    context.Key = id;
                    var processor = FlowBundle.GetFlowProcessor(FlowKey + "." + EntityControllerMethods.Delete);
                    var output = await processor.Run(Request);
                    scoep.Complete();
                    return output;
                }
            });

        }

        protected static TransactionScope GetScoep()
        {
            return new TransactionScope(TransactionScopeOption.Required, new TransactionOptions { IsolationLevel = IsolationLevel.ReadCommitted }, TransactionScopeAsyncFlowOption.Enabled);
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]TEntity value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetValidationErrorObject());
            }
            return await AuthorizationThen(EntityControllerMethods.Post, async () =>
                 {
                     using (var scoep = GetScoep())
                     {
                         var context = ServiceProvider.GetRequiredService<InputRequestContext<TEntity>>();
                         context.Input = value;
                         context.TransactionScope = scoep;
                         var processor = FlowBundle.GetFlowProcessor(FlowKey + "." + EntityControllerMethods.Post);
                         var output = await processor.Run(Request);
                         scoep.Complete();
                         return output;
                     }
                 });
        }



        [HttpPut("{id}")]
        public async Task<IActionResult> Put(TKey id, [FromBody]TEntity value)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(GetValidationErrorObject());
            }
            return await AuthorizationThen(EntityControllerMethods.Put, async () =>
                   {
                       using (var scoep = GetScoep())
                       {
                           var keyContext = ServiceProvider.GetRequiredService<KeyRequestContext<TKey>>();
                           keyContext.TransactionScope = scoep;
                           var context = ServiceProvider.GetRequiredService<InputRequestContext<TEntity>>();
                           context.TransactionScope = scoep;
                           context.Input = value;
                           keyContext.Key = id;
                           var processor = FlowBundle.GetFlowProcessor(FlowKey + "." + EntityControllerMethods.Put);
                           var output = await processor.Run(Request);
                           scoep.Complete();
                           return output;
                       }
                   });
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(TKey id)
        {
            return await AuthorizationThen(EntityControllerMethods.Get, async () =>
                   {
                       var context = ServiceProvider.GetRequiredService<KeyRequestContext<TKey>>();
                       context.Key = id;
                       var processor = FlowBundle.GetFlowProcessor(FlowKey + "." + EntityControllerMethods.Get);
                       var output = await processor.Run(Request);
                       return output;
                   });
        }


    }
}