using System;
using Newtonsoft.Json.Linq;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class JTokenCellWriter : CellWriterBase<JToken>, IPropertyCellWriter
    {
        private readonly string path;
        public Func<object, object> ValueTransform { get; set; }
        public Action<object, ExcelRange> PostValueWrite { get; set; }

        public JTokenCellWriter(string path)
        {
            this.path = path;
            Tag = path;
        }
        public override void Write(JToken entity, ExcelRange cell)
        {
            var token = entity.SelectToken(path);
            var value = token == null ? null : ((JValue)token).Value;
            if (ValueTransform != null)
            {
                value = ValueTransform(value);
            }
            cell.Value = value;
            PostValueWrite?.Invoke(value, cell);
        }
    }
}