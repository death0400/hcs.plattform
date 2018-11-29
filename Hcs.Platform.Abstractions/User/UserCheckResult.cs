namespace Hcs.Platform.User
{
    public class UserCheckResult
    {
        public bool Successed { get; }
        public string MessageCode { get; }
        public IPlatformUser User { get; }
        public UserCheckResult(bool successed, string messageCode, IPlatformUser user)
        {
            Successed = successed;
            MessageCode = messageCode;
            User = user;
        }
    }
}
