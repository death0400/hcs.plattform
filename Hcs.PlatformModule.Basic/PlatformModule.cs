using System.Linq;
using Hcs.Platform;
using Hcs.Platform.Data;
using Hcs.Platform.Flow;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.BaseModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Hcs.Platform.File;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Hcs.PlatformModule.Basic
{
    public class PlatformModule : IPlatformModule
    {
        public void Build(IPlatformModuleBuilder moduleBuilder)
        {
            //services
            moduleBuilder.Services.AddTransient<Processors.ClearUserCacheProcessor>();
            moduleBuilder.Services.AddTransient<Processors.UpdateGroupProcessor>();
            moduleBuilder.Services.AddTransient<Processors.UpdateRoleProcessor>();
            //apis
            moduleBuilder.ConfigDataProcessor<PlatformUser>(options => options.AddTask<PlatformUserEntityTask>());
            var user = moduleBuilder.AddEntityApi<long, PlatformUser>(options =>
            {
                options.ConfigGetApi(get => get.OnValidate(x => x.Pipe(Pipes.PlatformGroupUserPipes.ValidateUser)).OnGeted(geted => geted.Pipe(model => { model.Password = null; })));
                options.ConfigPostApi(post => post.OnValidate(validate => validate.Unique(unique =>
                {
                    unique.IsCreate = true;
                    unique.AddProperty(x => x.Account);
                })));

                options.ConfigPutApi(put =>
                {
                    put.OnValidate(validate => validate.Unique(unique => unique.AddProperty(x => x.Account)).Pipe(Pipes.PlatformGroupUserPipes.ValidateUser));
                    put.OnUpdated(updated => updated.Then<PlatformUser, Processors.ClearUserCacheProcessor>());
                });
                options.ConfigQueryApi(q =>
                {
                    q.OnQueryed(x => x.Pipe(Pipes.PlatformGroupUserPipes.FilterUser));
                    q.ConfigExportSetting(settings =>
                       {
                           settings.ConfigCellWriters(x =>
                           {
                               var w = x.Where(p => p.Tag == "Status").First();
                               w.ValueTransform = (v) => ((Platform.User.UserStatus)v) == Platform.User.UserStatus.Active ? "啟用" : "停用";
                           });
                       });
                });
                options.ConfigDeleteApi(delete => delete.OnValidate(x => x.Pipe(Pipes.PlatformGroupUserPipes.ValidateUserDelete)).OnDeleted(deleted => deleted.Then<PlatformUser, Processors.ClearUserCacheProcessor>()));
            });

            var updateGroupRole = moduleBuilder.AddPutFlowApi<long, Hcs.Platform.ViewModels.BatchUpdateView<PlatformGroupRole>>("updateRoles",
                builder => builder.GetKeyAndModel<long, Hcs.Platform.ViewModels.BatchUpdateView<PlatformGroupRole>>().Then<IActionResult, Processors.UpdateRoleProcessor>());

            var updateUserGroup = moduleBuilder.AddPutFlowApi<long, Hcs.Platform.ViewModels.BatchUpdateView<PlatformUserGroup>>("updateGroups",
                builder => builder.GetKeyAndModel<long, Hcs.Platform.ViewModels.BatchUpdateView<PlatformUserGroup>>().Then<IActionResult, Processors.UpdateGroupProcessor>());

            var groupRoleQuery = moduleBuilder.AddQueryApi<PlatformGroupRole>(opt => opt.OnQueryed(q => q.Pipe(Pipes.PlatformGroupUserPipes.FilterRoles)));
            var userGroupQuery = moduleBuilder.AddQueryApi<PlatformUserGroup>(opt => opt.OnQueryed(q => q.Pipe(Pipes.PlatformGroupUserPipes.FilterUserGroup)));
            var group = moduleBuilder.AddEntityApi<long, PlatformGroup>(options =>
            {
                options.ConfigGetApi(get => get.OnValidate(x => x.Pipe(Pipes.PlatformGroupUserPipes.ValidateGroup)));
                options.ConfigPutApi(put =>
                {
                    put.OnValidate(validate => validate.Pipe(Pipes.PlatformGroupUserPipes.ValidateGroup));
                });
                options.ConfigQueryApi(q =>
                {
                    q.OnQueryed(x => x.Pipe(Pipes.PlatformGroupUserPipes.FilterGroup));
                    q.ConfigExportSetting(settings =>
                       {
                           settings.ConfigCellWriters(x =>
                           {
                               var w = x.Where(p => p.Tag == "IsEnabled").First();
                               w.ValueTransform = (v) => ((bool)v) ? "是" : "否";
                           });
                       });
                });
                options.ConfigDeleteApi(delete => delete.OnValidate(x => x.Pipe(Pipes.PlatformGroupUserPipes.ValidateGroup)));
            });
            var getUsr = moduleBuilder.AddGetFlowApi<long, PlatformUser>("getmyinfo", builder =>
                 builder.GetService<Platform.User.IPlatformUser>()
                 .Pipe(x => x.Service.Id)
                 .GetData<long, PlatformUser>().Ok());
            var changePassword = moduleBuilder.AddPutFlowApi<long, Views.ChangePasswordView>("changepassword", builder =>
                builder.GetKeyAndModel<long, Views.ChangePasswordView>()
                .StartValidation()
                .Pipe(Pipes.ChangePasswordPipes.ValidateUser)
                .Pipe(Pipes.ChangePasswordPipes.ValidateOldPassword)
                .EndValidation(sbuilder => sbuilder.Pipe(Pipes.ChangePasswordPipes.ChangePassword).Ok()));
            //function
            var moduleName = "Basic";
            moduleBuilder.Everyone.AddRole(changePassword);
            moduleBuilder.Everyone.AddRole(getUsr);
            moduleBuilder.AddModuleFuncion(moduleName, "PlatformUser", options => options.AddStandardApiRoles(user, roleOptions =>
                    {
                        roleOptions.View.AddRole(userGroupQuery);
                        roleOptions.Modify.AddRole(updateUserGroup);
                    }).AddPermission("Admin", b =>
                    {

                    }));
            moduleBuilder.AddModuleFuncion(moduleName, "PlatformGroup", options => options.AddStandardApiRoles(group, roleOptions =>
            {
                roleOptions.View.AddRole(groupRoleQuery);
                roleOptions.Modify.AddRole(updateGroupRole);
            }));
        }
    }
}
namespace Microsoft.AspNetCore.Builder
{
    public static class PlatformModuleBasicExtensions
    {
        public static PlatformBuilder AddBasicModule(this PlatformBuilder builder)
        {
            builder.AddModule<Hcs.PlatformModule.Basic.PlatformModule>();
            return builder;
        }
    }
}