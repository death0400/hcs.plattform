using System;
using Hcs.Platform.User;

namespace Hcs.Platform.Abstractions.ViewModels
{
    public class PlatformUserInfo : IPlatformUser
    {
        public long Id { get; set; }

        public string Name { get; set; }
    }
}
