using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.AspNetCore.Mvc.ApplicationParts;
using Microsoft.AspNetCore.Mvc.Controllers;

namespace Hcs.Platform.Core.ApplicationFeatureProvider
{
    public class ControllerFeatureProvider : IApplicationFeatureProvider<ControllerFeature>
    {
        public void PopulateFeature(IEnumerable<ApplicationPart> parts, ControllerFeature feature)
        {
            foreach (var controller in PlatformModule.EntityApiContextContainer.Controllers)
            {
                feature.Controllers.Add(controller.GetTypeInfo());
            }

        }
    }
}