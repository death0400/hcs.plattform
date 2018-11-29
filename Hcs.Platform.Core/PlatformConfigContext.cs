namespace Hcs.Platform.Core
{
    public class PlatformConfigContext
    {
        internal PlatformConfigContext()
        {

        }
        public IJwtConfig JwtConfig { get; internal set; }
    }
}