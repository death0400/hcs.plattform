using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hcs.Platform.Data
{
    public interface IModelInfo<TContext, TModel> where TContext : DbContext where TModel : class
    {
        IEntityType EntityType { get; }
        IKey PrimaryKey { get; }
        Hcs.Expressions.IPropertyAccessor<TModel> PrimaryKeyAccessor { get; }
    }
    public interface IModelInfo<TContext, TModel, TKey> : IModelInfo<TContext, TModel> where TContext : DbContext where TModel : class
    {
        new Hcs.Expressions.IPropertyAccessor<TModel, TKey> PrimaryKeyAccessor { get; }
    }
}