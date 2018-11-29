using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Hcs.Platform.Core.PlatformModule;

namespace Hcs.Platform.Core.Controller
{
    public abstract class PlatformControllerBase : ControllerBase
    {
        protected Flow.IFlowBundle FlowBundle { get; }
        protected IServiceProvider ServiceProvider { get; }
        private readonly IAuthorizationService authorizationService;
        protected string FlowKey { get; }
        protected object GetValidationErrorObject()
        {
            return ModelState.ToDictionary(x => x.Key, x => x.Value.Errors.Select(y => new ValidationError { Title = x.Key, Message = y.ErrorMessage, Data = y.Exception?.ToString() }));
        }
        protected async Task<IActionResult> AuthorizationThen(string method, Func<Task<IActionResult>> process)
        {
            if (User.Identity.IsAuthenticated)
            {
                var authorizationResult = await authorizationService.AuthorizeAsync(User, new KeyRoleToken(FlowKey + "." + method), Policy.ControllerMethodRequirement.Requirement);
                if (authorizationResult.Succeeded)
                {
                    try
                    {
                        return await process();
                    }
                    catch (ValidationException ex)
                    {
                        return BadRequest(ex.ValidationErrors);
                    }
                }
                else
                {
                    return Forbid();
                }
            }
            else
            {
                return Challenge();
            }
        }
        public PlatformControllerBase(Flow.IFlowBundle flowBundle, IServiceProvider serviceProvider, IAuthorizationService authorizationService, IHttpContextAccessor context)
        {
            this.FlowBundle = flowBundle;
            this.ServiceProvider = serviceProvider;
            this.authorizationService = authorizationService;
            FlowKey = (string)context.HttpContext.Items["apiKey"];
        }
    }
}