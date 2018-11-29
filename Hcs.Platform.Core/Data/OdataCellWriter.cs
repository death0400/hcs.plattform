using System;
using System.Collections.Generic;
using Hcs.Serialize.Xlsx;
using Microsoft.AspNet.OData.Query;
using OfficeOpenXml;

namespace Hcs.Platform.Data
{
    public class OdataCellWriter : CellWriterBase<ISelectExpandWrapper>, IPropertyCellWriter
    {
        private readonly string[] path;

        public Func<object, object> ValueTransform { get; set; }
        public Action<object, ExcelRange> PostValueWrite { get; set; }

        public OdataCellWriter(string path)
        {
            this.path = path.Split('.');
            Tag = path;
        }
        public override void Write(ISelectExpandWrapper entity, ExcelRange cell)
        {
            var map = entity.ToDictionary();
            var q = new Queue<string>(path);
            object value = GetValue(q, map);
            if (ValueTransform != null)
            {
                value = ValueTransform(value);
            }
            cell.Value = value;
            PostValueWrite?.Invoke(value, cell);
        }
        static object GetValue(Queue<string> path, IDictionary<string, object> map)
        {
            var p = path.Dequeue();
            var value = map.ContainsKey(p) ? map[p] : null;
            if (path.Count > 0)
            {
                if (value == null)
                {
                    return null;
                }
                else
                {
                    return GetValue(path, ((ISelectExpandWrapper)value).ToDictionary());
                }
            }
            else
            {
                return value;
            }
        }
    }
}