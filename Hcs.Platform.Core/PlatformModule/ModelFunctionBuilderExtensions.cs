using System;

namespace Hcs.Platform.PlatformModule
{
    public static class ModelFunctionBuilderExtensions
    {
        public class StandardRoleContext
        {
            internal StandardRoleContext()
            {

            }
            public PermissionBuilder Create { get; internal set; }
            public PermissionBuilder Delete { get; internal set; }
            public PermissionBuilder Modify { get; internal set; }
            public PermissionBuilder View { get; internal set; }

        }
        public static ModelFunctionBuilder AddStandardApiRoles(this ModelFunctionBuilder builder, Data.EntityApiRoles apiRoles, Action<StandardRoleContext> config = null)
        {
            var context = new StandardRoleContext();
            builder.AddPermission(StandardRoles.Create, opt => context.Create = opt.AddRole(apiRoles.Post))
                .AddPermission(StandardRoles.Delete, opt => context.Delete = opt.AddRole(apiRoles.Delete))
                .AddPermission(StandardRoles.Modify, opt => context.Modify = opt.AddRole(apiRoles.Put))
                .AddPermission(StandardRoles.View, opt => context.View = opt.AddRole(apiRoles.Get).AddRole(apiRoles.Query));
            config?.Invoke(context);
            return builder;
        }
    }
}