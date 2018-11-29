using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public interface IPropertyCellWriter
    {
        string Tag { get; }
        Func<object, object> ValueTransform { get; set; }
        Action<object, ExcelRange> PostValueWrite { get; set; }
    }
}