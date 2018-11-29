using System.Collections.Generic;
using Hcs.Platform.Role;

namespace Hcs.Platform.Data
{
    public class EntityApiRoles
    {
        public Role.IRoleToken Get { get; internal set; }
        public Role.IRoleToken Put { get; internal set; }
        public Role.IRoleToken Delete { get; internal set; }
        public Role.IRoleToken Post { get; internal set; }

        public IRoleToken Query { get; internal set; }
        internal EntityApiRoles()
        {

        }
        public IEnumerable<Role.IRoleToken> All
        {
            get
            {
                if (Get != null)
                    yield return Get;
                if (Put != null)
                    yield return Put;
                if (Delete != null)
                    yield return Delete;
                if (Post != null)
                    yield return Post;
                if (Query != null)
                    yield return Post;
            }
        }

    }
}