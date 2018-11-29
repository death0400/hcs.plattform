using System;
namespace Hcs.Platform.Data
{

    public static partial class IPlatformModuleBuilderExtensions
    {
        public class EntityApiBuildContext<TKey, TEntity> where TEntity : class
        {
            internal EntityApiBuildContext()
            {

            }
            internal Action<GetApiConfigurations<TKey, TEntity>> GetConfig { get; private set; }
            public EntityApiBuildContext<TKey, TEntity> ConfigGetApi(Action<GetApiConfigurations<TKey, TEntity>> config)
            {
                if (GetConfig == null)
                {
                    GetConfig = config;
                }
                else
                {
                    var o = GetConfig;
                    GetConfig = x =>
                    {
                        o(x);
                        config(x);
                    };
                }
                return this;
            }
            internal Action<DeleteApiConfigurations<TKey, TEntity>> DeleteConfig { get; private set; }
            public EntityApiBuildContext<TKey, TEntity> ConfigDeleteApi(Action<DeleteApiConfigurations<TKey, TEntity>> config)
            {
                if (DeleteConfig == null)
                {
                    DeleteConfig = config;
                }
                else
                {
                    var o = DeleteConfig;
                    DeleteConfig = x =>
                    {
                        o(x);
                        config(x);
                    };
                }
                return this;
            }

            internal Action<QueryApiConfigurations<TEntity>> QueryConfig { get; private set; }
            public EntityApiBuildContext<TKey, TEntity> ConfigQueryApi(Action<QueryApiConfigurations<TEntity>> config)
            {
                if (QueryConfig == null)
                {
                    QueryConfig = config;
                }
                else
                {
                    var o = QueryConfig;
                    QueryConfig = x =>
                    {
                        o(x);
                        config(x);
                    };
                }
                return this;
            }
            internal Action<PostApiConfigurations<TKey, TEntity>> PostConfig { get; private set; }
            public EntityApiBuildContext<TKey, TEntity> ConfigPostApi(Action<PostApiConfigurations<TKey, TEntity>> config)
            {
                if (PostConfig == null)
                {
                    PostConfig = config;
                }
                else
                {
                    var o = PostConfig;
                    PostConfig = x =>
                    {
                        o(x);
                        config(x);
                    };
                }
                return this;
            }
            internal Action<PutApiConfigurations<TEntity>> PutConfig { get; private set; }
            public EntityApiBuildContext<TKey, TEntity> ConfigPutApi(Action<PutApiConfigurations<TEntity>> config)
            {
                if (PutConfig == null)
                {
                    PutConfig = config;
                }
                else
                {
                    var o = PutConfig;
                    PutConfig = x =>
                    {
                        o(x);
                        config(x);
                    };
                }
                return this;
            }
        }
    }
}