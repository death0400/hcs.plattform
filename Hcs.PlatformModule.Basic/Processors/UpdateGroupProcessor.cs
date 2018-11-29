using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Hcs.Extensions.EntityFrameworkCore;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Data;
using Hcs.Platform.User;
using Hcs.Platform.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Hcs.PlatformModule.Basic.Processors
{
    public class UpdateGroupProcessor : DbSetFlowProcessor<GetKeyAndModelOutput<long, BatchUpdateView<PlatformUserGroup>>, IActionResult, PlatformGroupRole, PlatformDbContext>
    {
        private readonly IPlatformUser user;
        private readonly System.Security.Principal.IPrincipal principal;
        public UpdateGroupProcessor(IScopedDbContext<PlatformDbContext> context, IPlatformUser user, System.Security.Principal.IPrincipal principal) : base(context)
        {
            this.principal = principal;
            this.user = user;
        }

        public override async Task ProcessAsync(GetKeyAndModelOutput<long, BatchUpdateView<PlatformUserGroup>> input, Action<IActionResult> done)
        {
            var isAdmin = principal.IsInRole("Basic.PlatformUser.Admin");
            var update = input.Entity.Entities;
            Expression<Func<PlatformUserGroup, bool>> scope = x => x.PlatformUserId == input.Key;
            if (!isAdmin)
            {
                if (input.Key == user.Id)
                {
                    done(new BadRequestResult());
                    return;
                }
                var ids = await Context.Set<PlatformGroup>().Where(x => x.PlatformUserGroups.Any(y => y.PlatformUserId == user.Id) || x.CreatedBy == user.Id).Select(x => x.Id).ToArrayAsync();
                update = update.Where(x => ids.Contains(x.PlatformGroupId)).ToArray();
                scope = x => x.PlatformUserId == input.Key && ids.Contains(x.PlatformGroupId);
            }

            Context.UpdateSet(update, scope, (a, b) => a.PlatformGroupId == b.PlatformGroupId && a.PlatformUserId == b.PlatformUserId);
            await Context.SaveChangesAsync();
            done(new OkResult());
        }
    }
}
