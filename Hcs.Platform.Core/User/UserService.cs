using System;
using System.Threading.Tasks;
using Hcs.Platform.Data;
using Hcs.Platform.ViewModels;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using Microsoft.Extensions.Caching.Memory;

namespace Hcs.Platform.User
{
    internal class UserService : IUserService
    {
        private readonly PlatformDbContext dbContext;
        private readonly IMemoryCache cache;

        public UserService(PlatformDbContext dbContext, IMemoryCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }
        public async Task<UserCheckResult> CheckLogin(LoginView login)
        {
            var set = dbContext.Set<Hcs.Platform.BaseModels.PlatformUser>();
            var user = await set.FirstOrDefaultAsync(x => x.Account == login.Account && x.Password == login.Password);
            string message = null;
            switch (user)
            {
                case null:
                    message = "Login.UserNotExists";
                    break;
                case var u when u.Status != UserStatus.Active:
                    message = "Login.UserNotActive";
                    break;

            }
            return new UserCheckResult(user != null && user.Status == UserStatus.Active, message, user);
        }

        public IPlatformUser Get(long id)
        {
            var key = Core.CacheKeyBuilder.User(id);
            return cache.GetOrCreate(key, entry =>
             {
                 entry.AbsoluteExpirationRelativeToNow = new TimeSpan(0, 10, 0);
                 var set = dbContext.Set<Hcs.Platform.BaseModels.PlatformUser>();
                 return set.Find(id);
             });
        }

    }
}
