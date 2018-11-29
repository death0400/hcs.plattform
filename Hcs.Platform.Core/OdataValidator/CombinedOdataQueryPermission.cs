using System;
using System.Collections.Generic;
using System.Linq;
namespace Hcs.Platform.Odata
{
    [Serializable]
    internal class CombinedOdataQueryPermission : IOdataQueryPermission
    {
        Dictionary<Type, HashSet<string>> allows = new Dictionary<Type, HashSet<string>>();
        public Type EntityType => throw new NotImplementedException();

        public CombinedOdataQueryPermission(params IOdataQueryPermission[] permissions)
        {
            foreach (var g in permissions.SelectMany(x => x.GettAll()).GroupBy(x => x.Key))
            {
                allows.Add(g.Key, new HashSet<string>(g.Select(x => x.Value).Distinct()));
            }
        }

        public IEnumerable<string> GetByEntityType(Type type)
        {
            if (allows.ContainsKey(type))
            {
                return allows[type];
            }
            else
            {
                return new string[] { };
            }
        }

        public IEnumerable<KeyValuePair<Type, string>> GettAll()
        {
            foreach (var a in allows)
            {
                foreach (var v in a.Value)
                {
                    yield return new KeyValuePair<Type, string>(a.Key, v);
                }
            }
        }
    }
}
