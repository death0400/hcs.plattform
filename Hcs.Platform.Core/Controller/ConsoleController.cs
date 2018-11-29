using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Security;
using Hcs.Platform.PlatformModule;
using Hcs.Platform.User;
using Microsoft.AspNetCore.Mvc;
using Hcs.Extensions.EntityFrameworkCore;
using Hcs.Platform.Abstractions.Role;

namespace Hcs.Platform.Core.Controller
{
    [Route("/api/console")]
    public class ConsoleController : ControllerBase
    {
        private readonly Platform.Data.PlatformDbContext dbContext;
        private readonly IPasswordHashService passwordHashService;
        private readonly IPlatformFunctionService functionService;

        public ConsoleController(Hcs.Platform.Data.PlatformDbContext dbContext, IPasswordHashService passwordHashService, IPlatformFunctionService functionService)
        {
            this.functionService = functionService;
            this.passwordHashService = passwordHashService;
            this.dbContext = dbContext;

        }
        [HttpGet("newadmin/{account}/{password}")]
        public async Task<IActionResult> Get(string account, string password)
        {
            if (!IsLocalRequest())
            {
                return NotFound();
            }
            var set = dbContext.Set<PlatformUser>();
            if (set.Any(x => x.Account == account))
            {
                return BadRequest("account exists");
            }
            var newUser = new PlatformUser
            {
                Account = account,
                Password = passwordHashService.Hash(password),
                Name = account,
                Status = UserStatus.Active
            };
            await set.AddAsync(newUser);
            var flagSet = dbContext.Set<PlatformFlag>();
            var groupSet = dbContext.Set<PlatformGroup>();
            var groupFlag = await flagSet.FindAsync("SuperGroup");
            PlatformGroup group;
            if (groupFlag == null)
            {
                group = new PlatformGroup { Name = "Administrator", IsEnabled = true };
                await groupSet.AddAsync(group);
                await dbContext.SaveChangesAsync();
                groupFlag = new PlatformFlag { Flag = "SuperGroup", Value = group.Id.ToString() };
                await flagSet.AddAsync(groupFlag);
            }
            else
            {
                group = await groupSet.FindAsync(long.Parse(groupFlag.Value));
            }
            var roleSet = dbContext.Set<PlatformGroupRole>();
            var roles = functionService.Functions.SelectMany(x => x.Permissions.Select(y => new { Function = x, Permission = y })).Select(x => new PlatformGroupRole
            {
                FunctionRoleCode = x.Permission.PermissionCode,
                FunctionCode = x.Function.Code,
                Permission = PermissionStatus.Granted,
                PlatformGroupId = group.Id
            });
            dbContext.UpdateSet(roles,
            x => x.PlatformGroupId == group.Id, (x, y) => x.FunctionCode == y.FunctionCode && x.FunctionRoleCode == y.FunctionRoleCode,
            (o, n) => o.Permission = n.Permission
            );
            await dbContext.Set<PlatformUserGroup>().AddAsync(new PlatformUserGroup { PlatformGroup = group, PlatformUser = newUser });
            await dbContext.SaveChangesAsync();
            return Ok();
        }
        bool IsLocalRequest()
        {
            var context = HttpContext;
            if (context.Connection.RemoteIpAddress.Equals(context.Connection.LocalIpAddress))
            {
                return true;
            }
            if (IPAddress.IsLoopback(context.Connection.RemoteIpAddress))
            {
                return true;
            }
            return false;
        }
    }

}