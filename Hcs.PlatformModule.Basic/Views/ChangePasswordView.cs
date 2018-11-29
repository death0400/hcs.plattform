using System.ComponentModel.DataAnnotations;

namespace Hcs.PlatformModule.Basic.Views
{
    public class ChangePasswordView
    {
        [Key]
        public long Id { get; set; }
        public string Password { get; set; }
        public string OldPassword { get; set; }
    }
}