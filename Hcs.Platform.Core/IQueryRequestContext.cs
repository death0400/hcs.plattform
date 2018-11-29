using System.Transactions;
using Microsoft.AspNet.OData.Query;
using Microsoft.AspNetCore.Http;

namespace Hcs.Platform.Core
{
    public abstract class RequestContextBase : IRequestContext
    {
        public TransactionScope TransactionScope { get; internal set; }
    }

    public class OdataQueryRequestContext<TEntity> : RequestContextBase where TEntity : class
    {


        public ODataQueryOptions<TEntity> ODataQueryOptions { get; internal set; }
    }
    public class KeyRequestContext<TKey> : RequestContextBase
    {


        public TKey Key { get; internal set; }
    }
    public class InputRequestContext<TInput> : RequestContextBase
    {
        public TInput Input { get; internal set; }
    }
}