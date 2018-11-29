using System;
using System.Collections.Generic;
namespace Hcs.Platform.Odata
{
    public interface IOdataQueryPermission
    {
        IEnumerable<string> GetByEntityType(Type type);
        IEnumerable<KeyValuePair<Type, string>> GettAll();
    }
}
