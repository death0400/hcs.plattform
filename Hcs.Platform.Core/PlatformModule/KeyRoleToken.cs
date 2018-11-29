namespace Hcs.Platform.Core.PlatformModule
{
    internal class KeyRoleToken : Role.IRoleToken
    {
        private readonly string key;

        public KeyRoleToken(string key)
        {
            this.key = key;
        }
        public string Name => key;
    }
}