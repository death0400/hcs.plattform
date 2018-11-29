using System;
using Hcs.Platform.Core;
using Hcs.Platform.PlatformModule;
using Microsoft.EntityFrameworkCore;

namespace Hcs.Platform
{

    public static class IPlatformBuilderExtensions
    {
        public static PlatformBuilder AddModule<TModule>(this PlatformBuilder builder) where TModule : class, IPlatformModule, new()
        {
            return builder.AddModule(new TModule());
        }

    }
}