using System;
using System.Linq;
using Hcs.Platform.PlatformModule;
using Microsoft.AspNet.OData;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.Core.Controller
{
    [Authorize]
    public class PlatformFunctionController : ControllerBase
    {
        private readonly IPlatformFunctionService functionService;

        public PlatformFunctionController(IPlatformFunctionService functionService)
        {
            this.functionService = functionService;
        }
        [EnableQuery]
        public IQueryable<IPlatformFunction> Get()
        {
            return functionService.Functions.AsQueryable();
        }
    }
}
