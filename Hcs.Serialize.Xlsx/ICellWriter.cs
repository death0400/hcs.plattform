using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public interface ICellWriter<T>
    {
        string Tag { get; }
        int CellWidth { get; set; }
        int CellHeight { get; set; }
        void Write(T entity, ExcelRange cell);
        Action<T, ExcelRange> PreWrite { get; }
        Action<T, ExcelRange> PostWrite { get; }
    }
}