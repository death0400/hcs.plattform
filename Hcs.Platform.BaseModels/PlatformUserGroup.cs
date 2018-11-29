using System;
using Hcs.Platform.Data;
using Hcs.Platform.User;

namespace Hcs.Platform.BaseModels
{
    public class PlatformUserGroup
    {
        public long PlatformUserId { get; set; }
        public long PlatformGroupId { get; set; }
        public PlatformUser PlatformUser { get; set; }
        public PlatformGroup PlatformGroup { get; set; }

    }
}