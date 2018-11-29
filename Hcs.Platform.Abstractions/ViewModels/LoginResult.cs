using System;

namespace Hcs.Platform.ViewModels
{
    public class LoginResult
    {
        public User.IPlatformUser User { get; set; }
        public string MessageCode { get; set; }
        public bool Succeeded { get; set; }

        public string Token { get; set; }
        public string[] Roles { get; set; }
    }
}
