using System;
using System.Threading.Tasks;

namespace Hcs.Platform.User
{
    public interface IUserService
    {
        IPlatformUser Get(long id);
        Task<UserCheckResult> CheckLogin(ViewModels.LoginView login);
    }
}
