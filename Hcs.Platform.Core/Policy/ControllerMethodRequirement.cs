using System;
using Microsoft.AspNetCore.Authorization;

namespace Hcs.Platform.Policy
{
    public class ControllerMethodRequirement : IAuthorizationRequirement
    {
        public static ControllerMethodRequirement Requirement { get; } = new ControllerMethodRequirement();
    }
}
