using System;
using Hcs.Platform.BaseModels;
using Hcs.Platform.Flow;

namespace Hcs.PlatformModule.Basic.Processors
{
    public class ClearUserCacheProcessor : FlowProcessor<PlatformUser, PlatformUser>
    {
        private readonly Platform.User.IUserCacheManager userCache;

        public ClearUserCacheProcessor(Hcs.Platform.User.IUserCacheManager userCache)
        {
            this.userCache = userCache;
        }
        public override PlatformUser Process(PlatformUser input)
        {
            userCache.Clear(input.Id);
            return input;
        }
    }
}
