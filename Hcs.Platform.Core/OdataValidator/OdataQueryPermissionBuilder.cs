using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace Hcs.Platform.Odata
{
    public class OdataQueryPermissionBuilder<TEntity> : IOdataQueryPermission
    {
        internal OdataQueryPermissionBuilder()
        {

        }
        List<string> allowExpandPaths = new List<string>();
        public Type EntityType => typeof(TEntity);

        public OdataQueryPermissionBuilder<TEntity> AllowExpand(Expression<Func<TEntity, object>> path)
        {
            allowExpandPaths.Add(string.Join(".", path.GetPropertyVistPath().Select(x => x.Name)));
            return this;
        }

        public IEnumerable<string> GetByEntityType(Type type)
        {
            if (type == typeof(TEntity))
            {
                return allowExpandPaths;
            }
            else
            {
                return new string[] { };
            }
        }

        public IEnumerable<KeyValuePair<Type, string>> GettAll()
        {
            return allowExpandPaths.Select(x => new KeyValuePair<Type, string>(EntityType, x));
        }
    }
}
