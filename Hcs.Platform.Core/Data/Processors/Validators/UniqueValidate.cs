using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.ComponentModel.DataAnnotations;
using Hcs.Platform.Core;

namespace Hcs.Platform.Data.Processors.Validators
{
    public class UniqueValidateConfigurator<TEntity, TDbContext> where TDbContext : DbContext where TEntity : class
    {
        private readonly UniqueValidate<TEntity, TDbContext> validator;

        internal UniqueValidateConfigurator(UniqueValidate<TEntity, TDbContext> validator)
        {
            this.validator = validator;
        }
        public bool NullCheck { get => validator.NullCheck; set => validator.NullCheck = value; }
        public bool IsCreate { get => validator.IsCreate; set => validator.IsCreate = value; }

        public void AddProperty<TResult>(Expression<Func<TEntity, TResult>> property)
        {
            validator.AddProperty<TResult>(property);
        }
    }
    class UniqueValidate<TEntity, TDbContext> : ValidationProcessor<TEntity> where TDbContext : DbContext where TEntity : class
    {
        protected class Pinfo
        {
            public string FieldName { get; set; }
            public Type FieldType { get; set; }

            public Func<TEntity, object> Compiled { get; set; }
            public LambdaExpression PropertySelector { get; set; }
        }
        protected ParameterExpression Parameter { get; set; }
        public bool NullCheck { get; set; }
        protected Lazy<List<Pinfo>> _pinfo;
        protected Lazy<List<Pinfo>> _keyPinfo;
        public bool IsCreate { get; set; }

        protected List<Pinfo> Pinfos => _pinfo.Value;
        protected List<LambdaExpression> PropertySelectors { get; set; } = new List<LambdaExpression>();
        protected List<Pinfo> KeyPinfo => _keyPinfo.Value;
        public UniqueValidate<TEntity, TDbContext> AddProperty<TResult>(Expression<Func<TEntity, TResult>> property)
        {
            if (Parameter == null)
            {
                Parameter = property.Parameters.Single();
            }
            PropertySelectors.Add(property);

            return this;
        }
        DbSet<TEntity> set;
        internal UniqueValidate(IScopedDbContext<TDbContext> context)
        {
            set = context.DbContext.Set<TEntity>();
            _keyPinfo = new Lazy<List<Pinfo>>(() =>
            {
                var pinfos = new List<Pinfo>();
                var keyProperties = context.DbContext.Model.FindEntityType(typeof(TEntity)).GetKeys().SelectMany(x => x.Properties).Select(x => typeof(TEntity).GetProperty(x.Name));
                foreach (var k in keyProperties)
                {
                    var pinfo = new Pinfo();
                    pinfo.PropertySelector = Expression.Lambda(Expression.Property(Parameter, typeof(TEntity).GetProperty(k.Name)), Parameter);
                    pinfo.Compiled = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(Expression.Invoke(pinfo.PropertySelector, Parameter), typeof(object)), Parameter).Compile();
                    pinfo.FieldName = k.Name;
                    pinfo.FieldType = k.PropertyType;
                    pinfos.Add(pinfo);
                }
                return pinfos;
            });
            _pinfo = new Lazy<List<Pinfo>>(() =>
              {
                  var pinfos = new List<Pinfo>();

                  foreach (var propertySelector in PropertySelectors)
                  {
                      var pinfo = new Pinfo();
                      if (propertySelector.Parameters.First() != Parameter)
                      {
                          pinfo.PropertySelector = propertySelector.ReplaceParameter(propertySelector.Parameters.First(), Parameter);
                      }
                      else
                      {
                          pinfo.PropertySelector = propertySelector;
                      }
                      var pathes = propertySelector.GetPropertyVistPath();
                      pinfo.Compiled = Expression.Lambda<Func<TEntity, object>>(Expression.Convert(Expression.Invoke(propertySelector, Parameter), typeof(object)), Parameter).Compile();
                      pinfo.FieldType = (pathes.Last() as PropertyInfo).PropertyType;
                      pinfo.FieldName = string.Join(".", pathes.Select(x => x.Name));
                      pinfos.Add(pinfo);
                  }
                  return pinfos;
              });
        }



        public override async Task Validate(TEntity entity, Action<ValidationError> addError)
        {
            var errors = new List<ValidationError>();
            if (NullCheck || Pinfos.Any(x => x.Compiled(entity) != null))
            {
                var filters = Pinfos.Select(pinfo =>
                {
                    var obj = new { value = pinfo.Compiled(entity) };
                    var right = Expression.Convert(Expression.Property(Expression.Constant(obj), "value"), pinfo.FieldType);
                    return Expression.Equal(pinfo.PropertySelector.Body, right);
                });

                BinaryExpression filter = filters.Aggregate((c, n) => Expression.AndAlso(c, n));

                Expression<Func<TEntity, bool>> where;
                if (IsCreate)
                {
                    where = Expression.Lambda<Func<TEntity, bool>>(filter, Parameter);
                }
                else
                {
                    var keys = KeyPinfo.Select(pinfo =>
                    {
                        var obj = new { value = pinfo.Compiled(entity) };
                        var right = Expression.Convert(Expression.Property(Expression.Constant(obj), "value"), pinfo.FieldType);
                        return Expression.Equal(pinfo.PropertySelector.Body, right);
                    });
                    BinaryExpression equal = keys.Aggregate((c, n) => Expression.AndAlso(c, n));
                    where = Expression.Lambda<Func<TEntity, bool>>(Expression.And(Expression.Not(equal), filter), Parameter);
                }

                if (await set.AnyAsync(where))
                {
                    errors.AddRange(Pinfos.Select(x => new ValidationError { Title = x.FieldName, Message = "duplicate" }));
                }
            }
            if (errors.Any())
            {
                foreach (var err in errors)
                {
                    addError(err);
                }
            }
        }
    }
}