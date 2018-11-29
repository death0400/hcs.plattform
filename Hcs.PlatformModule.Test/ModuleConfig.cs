using System;
using System.Threading.Tasks;
using Hcs.Platform.Data;
using Hcs.Platform.PlatformModule;
using Microsoft.EntityFrameworkCore;

namespace Hcs.PlatformModule.Test
{
    public class ModuleConfig : IPlatformModule
    {
        public static Action<Model.Entities.Customer> Pipe1 = c => { };
        public static Func<Model.Entities.Customer, Task> Pipe2 = async c => { await Task.CompletedTask; };
        public static Action<DbContext, Model.Entities.Customer> Pipe3 = (ctx, c) => { };
        public static Func<DbContext, Model.Entities.Customer, Task> Pipe4 = async (ctx, c) => { await Task.CompletedTask; };
        public static Action<DbContext, IScopedDbContext<DbContext>, Model.Entities.Customer> Pipe5 = (ctx, sctx, c) => { };
        public static Func<DbContext, IScopedDbContext<DbContext>, Model.Entities.Customer, Task> Pipe6 = async (ctx, sctx, c) => { await Task.CompletedTask; };
        public void Build(IPlatformModuleBuilder moduleBuilder)
        {
            moduleBuilder.AddModel<Model.ModelConfig>();

            var customer = moduleBuilder.AddEntityApi<long, Model.Entities.Customer>(options =>
            {
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe1)));
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe2)));
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe3)));
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe4)));
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe5)));
                options.ConfigDeleteApi(x => x.OnDeleted(d => d.Pipe(Pipe6)));

                options.ConfigGetApi(x => x.OnGeted(y => y.QueryChildFor(z => z.Orders)));
                options.ConfigPostApi(x => x.OnCreated(y => y.SaveChildsFor(z => z.Orders)));
                options.ConfigPutApi(x => x.OnUpdated(y => y.SaveChildsFor(z => z.Orders)));
            });
            var space = "PlatformModule.Test";
            moduleBuilder.AddModuleFuncion($"{space}.{nameof(Model.Entities.Customer)}", options =>
            {
                options.AddStandardApiRoles(customer, o => o.View.AddOdataPermission<Model.Entities.Customer>(build => build.AllowExpand(x => x.CreatedByUser.Name)));
            });
        }
    }
}
