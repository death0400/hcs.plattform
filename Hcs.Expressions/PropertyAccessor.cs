using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Linq.Expressions;
namespace Hcs.Expressions
{

    public class PropertyAccessor<T, TProperty> : IPropertyAccessor<T, TProperty>
    {
        public Action<T, TProperty> Setter { get; }
        public Func<T, TProperty> Getter { get; }
        void IPropertyAccessor<T, TProperty>.SetValue(T instance, TProperty value)
        {
            Setter(instance, value);
        }
        TProperty IPropertyAccessor<T, TProperty>.GetValue(T instance)
        {
            return Getter(instance);
        }
        void IPropertyAccessor<T>.SetValue(T instance, object value)
        {
            Setter(instance, value == null ? default(TProperty) : (TProperty)value);
        }
        object IPropertyAccessor<T>.GetValue(T instance)
        {
            return Getter(instance);
        }

        public bool IsNotDefault(T instance)
        {
            var v = Getter(instance);
            return v != null && !v.Equals(default(TProperty));
        }

        public PropertyAccessor(string propertyName)
        {
            var propertyInfo = typeof(T).GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"{propertyName} not found in {typeof(T).FullName}");
            }
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(T));
            var valueParameter = System.Linq.Expressions.Expression.Parameter(typeof(TProperty));
            var property = System.Linq.Expressions.Expression.Property(parameter, propertyInfo);
            if (propertyInfo.CanWrite)
            {
                Setter = System.Linq.Expressions.Expression.Lambda<Action<T, TProperty>>(System.Linq.Expressions.Expression.Assign(property, valueParameter), parameter, valueParameter).Compile();
            }
            if (propertyInfo.CanRead)
            {
                Getter = System.Linq.Expressions.Expression.Lambda<Func<T, TProperty>>(property, parameter).Compile();
            }
        }
    }
    public class PropertyAccessor : IPropertyAccessor<object, object>
    {
        readonly Type propertyType;
        public Action<object, object> Setter { get; }
        public Func<object, object> Getter { get; }
        public void SetValue(object instance, object value)
        {
            if (value != null && value.GetType() != propertyType)
            {
                value = Convert.ChangeType(value, propertyType);
            }
            else if (value == null && propertyType.IsValueType)
            {
                value = Activator.CreateInstance(propertyType);
            }
            Setter(instance, value);
        }
        public object GetValue(object instance)
        {
            return Getter(instance);
        }
        public bool IsNotDefault(object instance)
        {
            var v = Getter(instance);
            if (instance == null)
            {
                var type = instance.GetType();
                if (type.IsValueType)
                {
                    return instance != Activator.CreateInstance(type);
                }
                else
                {
                    return instance != null;
                }
            }
            else
            {
                return false;
            }
        }
        public PropertyAccessor(Type type, string propertyName)
        {
            var propertyInfo = type.GetProperty(propertyName);
            if (propertyInfo == null)
            {
                throw new InvalidOperationException($"{propertyName} not found in {type.FullName}");
            }
            propertyType = propertyInfo.PropertyType;
            var parameter = System.Linq.Expressions.Expression.Parameter(typeof(object));
            var valueParameter = System.Linq.Expressions.Expression.Parameter(typeof(object));
            var property = System.Linq.Expressions.Expression.Property(System.Linq.Expressions.Expression.Convert(parameter, type), propertyName);
            if (propertyInfo.CanWrite)
            {
                Setter = System.Linq.Expressions.Expression.Lambda<Action<object, object>>(System.Linq.Expressions.Expression.Assign(property, System.Linq.Expressions.Expression.Convert(valueParameter, propertyType)), parameter, valueParameter).Compile();
            }
            if (propertyInfo.CanRead)
            {
                Getter = System.Linq.Expressions.Expression.Lambda<Func<object, object>>(System.Linq.Expressions.Expression.Convert(property, typeof(object)), parameter).Compile();
            }
        }
    }
}
