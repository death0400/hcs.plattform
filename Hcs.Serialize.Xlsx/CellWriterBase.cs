using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public abstract class CellWriterBase<T> : ICellWriter<T>
    {
        public virtual int CellWidth { get; set; } = 1;
        public virtual int CellHeight { get; set; } = 1;
        public virtual string Tag { get; set; }
        public virtual Action<T, ExcelRange> PreWrite { get; set; }
        public virtual Action<T, ExcelRange> PostWrite { get; set; }
        public abstract void Write(T entity, ExcelRange cell);
    }
}