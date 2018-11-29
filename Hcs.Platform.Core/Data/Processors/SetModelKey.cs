using System;
using Microsoft.EntityFrameworkCore;
namespace Hcs.Platform.Data
{
    public class SetModelKey<TKey, TEntity, TDbContext> : Flow.FlowProcessor<TEntity, TEntity> where TDbContext : DbContext where TEntity : class
    {
        private readonly Core.KeyRequestContext<TKey> keyContext;
        private readonly IModelInfo<TDbContext, TEntity, TKey> info;

        public SetModelKey(TDbContext context, Core.KeyRequestContext<TKey> keyContext, IModelInfo<TDbContext, TEntity, TKey> info)
        {
            this.keyContext = keyContext;
            this.info = info;
        }
        public override TEntity Process(TEntity input)
        {
            info.PrimaryKeyAccessor.SetValue(input, keyContext.Key);
            return input;
        }
    }
}