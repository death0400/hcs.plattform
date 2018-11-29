using System;
using Hcs.Platform;
using Hcs.Platform.Core;
using Hcs.Platform.Core.Data.DataProcessorEntityTasks.BuiltInTasks;
using Hcs.Platform.Security;
using Hcs.Platform.Data;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.User;
using Microsoft.Extensions.DependencyInjection;
using Hcs.Platform.Role;
using Hcs.Platform.Flow;
using System.Linq;
using Hcs.Platform.Abstractions.ViewModels;

namespace Hcs.Platform.Core
{
    static class DefaultPlatformConfig
    {
        /// <summary>
        /// add platform components here
        /// </summary>
        /// <param name="builder"></param>
        public static void DefaultConfig(PlatformBuilder builder)
        {
            builder.AddModule<PlatformModule>();
        }
        public class PlatformModule : IPlatformModule
        {
            public void Build(IPlatformModuleBuilder moduleBuilder)
            {
                moduleBuilder.ConfigDataProcessor<IPlatformEntity>(options => options.AddTask(typeof(PlatformEntityTask<>)));
                moduleBuilder.AddModel<Hcs.Platform.BaseModels.ModelConfig>();
                var getRoles = moduleBuilder.AddQueryFlowApi<IPlatformFunction>("functions", builder =>
                builder.GetService<IUserPlatformRoleAccessor>()
                .Pipe(x => x.Service.Roles.Select(s => new { Code = s.Code.Substring(0, s.Code.Length - s.PermissionCode.Length - 1), PermissionCode = s.PermissionCode }).Where(f => f.Code != "Everyone").GroupBy(g => g.Code).Select(s => new { Code = s.Key, Permissions = s.Select(p => p.PermissionCode) }))
                .Ok());
                var getUser = moduleBuilder.AddGetFlowApi<long, PlatformUserInfo>("user", builder => builder.GetRequestKey<long>().GetData<long, BaseModels.PlatformUser>().Pipe(x => new PlatformUserInfo { Id = x.Id, Name = x.Name }).Ok());
                moduleBuilder.Everyone.AddRole(getRoles);
                moduleBuilder.Everyone.AddRole(getUser);
            }
        }
    }
}