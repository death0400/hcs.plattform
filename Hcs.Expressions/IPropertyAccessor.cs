using System;
namespace Hcs.Expressions
{
    public interface IPropertyAccessor<T>
    {
        void SetValue(T instance, object value);
        object GetValue(T instance);
        bool IsNotDefault(T instance);
    }
    public interface IPropertyAccessor<T, TProperty> : IPropertyAccessor<T>
    {
        Action<T, TProperty> Setter { get; }
        Func<T, TProperty> Getter { get; }
        void SetValue(T instance, TProperty value);
        new TProperty GetValue(T instance);
    }
}
