using System;
using System.Linq;
using System.Linq.Expressions;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class PropertyCellWriter<T> : CellWriterBase<T>, IPropertyCellWriter
    {
        private readonly Func<T, object> property;
        public Func<object, object> ValueTransform { get; set; }
        public Action<object, ExcelRange> PostValueWrite { get; set; }

        public override void Write(T entity, ExcelRange cell)
        {
            var value = property(entity);
            if (ValueTransform != null)
            {
                value = ValueTransform(value);
            }
            cell.Value = value;
            PostValueWrite?.Invoke(value, cell);
        }
        public PropertyCellWriter(Func<T, object> property)
        {
            this.property = property;
        }
        public PropertyCellWriter(string propertyPath, Type t)
        {
            Tag = propertyPath;
            var path = propertyPath.Split('.');
            var parameter = Expression.Parameter(typeof(T), "entity");
            Expression body = Expression.PropertyOrField(Expression.Convert(parameter, t), path.First());
            foreach (var p in path.Skip(1))
            {
                var member = Expression.PropertyOrField(body, p);
                body = Expression.Condition(Expression.Equal(body, Expression.Constant(null)), Expression.Constant(null, member.Type), member);
            }
            property = Expression.Lambda<Func<T, object>>(Expression.Convert(body, typeof(object)), parameter).Compile();
        }
    }
}