using System;
using Microsoft.AspNetCore.Mvc.ApplicationModels;

namespace Hcs.Platform.Core.Controller
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class GenericControllerNameConvention : Attribute, IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            if (controller.ControllerType.IsGenericType)
            {
                controller.ControllerName = controller.ControllerType.GetFriendlyFullName();
            }
        }
    }
}