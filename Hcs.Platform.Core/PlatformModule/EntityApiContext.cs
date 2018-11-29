using System;
using System.Linq;
using Hcs.Platform.Flow;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Hcs.Platform.PlatformModule
{
    public sealed class EntityApiContext
    {
        internal DIFlowBuilderHandler<HttpRequest, IActionResult> BuildFlow { get; }
        public Type ControllerType { get; }
        public string ControllerName { get; }
        public string ApiKey { get; }
        public EntityApiContext(string apikey, string flowKey, DIFlowBuilderHandler<HttpRequest, IActionResult> buildFlow, Type controllerType)
        {
            Key = flowKey;
            ApiKey = apikey;
            this.BuildFlow = buildFlow;
            ControllerType = controllerType;
            ControllerName = controllerType.GetFriendlyFullName();
        }
        public string Key { get; }
    }
}