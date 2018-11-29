using System;
using System.Linq;
using System.Threading.Tasks;
using Hcs.Extensions.EntityFrameworkCore;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Data;
using Hcs.Platform.User;
using Hcs.Platform.ViewModels;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
namespace Hcs.PlatformModule.Basic.Processors
{
    public class UpdateRoleProcessor : DbSetFlowProcessor<GetKeyAndModelOutput<long, BatchUpdateView<PlatformGroupRole>>, IActionResult, PlatformGroupRole, PlatformDbContext>
    {
        private readonly IPlatformUser user;
        private readonly System.Security.Principal.IPrincipal principal;
        public UpdateRoleProcessor(IScopedDbContext<PlatformDbContext> context, IPlatformUser user, System.Security.Principal.IPrincipal principal) : base(context)
        {
            this.principal = principal;
            this.user = user;
        }

        public override async Task ProcessAsync(GetKeyAndModelOutput<long, BatchUpdateView<PlatformGroupRole>> input, Action<IActionResult> done)
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            var update = input.Entity.Entities;
            Expression<Func<PlatformGroupRole, bool>> scope = x => x.PlatformGroupId == input.Key;
            if (!isAdmin)
            {
                var ids = await Context.Set<PlatformGroup>().Where(x => x.PlatformUserGroups.Any(y => y.PlatformUserId == user.Id) || x.CreatedBy == user.Id).Select(x => x.Id).ToArrayAsync();
                update = update.Where(x => ids.Contains(x.PlatformGroupId)).ToArray();
                scope = x => x.PlatformGroupId == input.Key && ids.Contains(x.PlatformGroupId);
            }
            Context.UpdateSet(update, scope, (a, b) => a.FunctionCode == b.FunctionCode && a.FunctionRoleCode == b.FunctionRoleCode, (o, n) =>
            {
                o.Permission = n.Permission;
            });
            await Context.SaveChangesAsync();
            done(new OkResult());
        }
    }
}
