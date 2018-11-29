using System;
using System.Linq;
using System.Reflection;
using Hcs.Expressions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace Hcs.Platform.Data
{
    class ModelInfo<TContext, TModel> : IModelInfo<TContext, TModel> where TContext : DbContext where TModel : class
    {
        public ModelInfo(TContext context)
        {
            EntityType = context.Model.FindEntityType(typeof(TModel));
            if (primaryKey == null)
            {
                lock (lockobj)
                {
                    if (primaryKey == null)
                    {
                        primaryKey = EntityType.FindPrimaryKey();
                        var type = typeof(Hcs.Expressions.PropertyAccessor<,>).MakeGenericType(typeof(TModel), primaryKey.Properties[0].PropertyInfo.PropertyType);
                        primaryKeyAccessor = (IPropertyAccessor<TModel>)Activator.CreateInstance(type, PrimaryKey.Properties[0].Name);
                    }
                }
            }

        }
        static object lockobj = new object();
        static IKey primaryKey;
        static IPropertyAccessor<TModel> primaryKeyAccessor;
        public IKey PrimaryKey => primaryKey;

        public IPropertyAccessor<TModel> PrimaryKeyAccessor => primaryKeyAccessor;

        public IEntityType EntityType { get; }
    }
    class ModelInfo<TContext, TModel, TKey> : IModelInfo<TContext, TModel, TKey> where TContext : DbContext where TModel : class
    {
        public IEntityType EntityType { get; }
        public ModelInfo(TContext context)
        {
            EntityType = context.Model.FindEntityType(typeof(TModel));
            if (primaryKey == null)
            {
                lock (lockobj)
                {
                    if (primaryKey == null)
                    {
                        primaryKey = EntityType.FindPrimaryKey();
                        primaryKeyAccessor = new Hcs.Expressions.PropertyAccessor<TModel, TKey>(PrimaryKey.Properties[0].Name);
                    }
                }
            }

        }
        static object lockobj = new object();
        static IKey primaryKey;
        static IPropertyAccessor<TModel, TKey> primaryKeyAccessor;
        public IKey PrimaryKey => primaryKey;

        public IPropertyAccessor<TModel, TKey> PrimaryKeyAccessor => primaryKeyAccessor;

        IPropertyAccessor<TModel> IModelInfo<TContext, TModel>.PrimaryKeyAccessor => PrimaryKeyAccessor;
    }
}