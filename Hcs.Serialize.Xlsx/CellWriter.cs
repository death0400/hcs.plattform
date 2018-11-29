using System;
using System.Linq.Expressions;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class CellWriter<T> : CellWriterBase<T>
    {
        private readonly Action<T, ExcelRange> write;
        public override void Write(T entity, ExcelRange cell)
        {
            write(entity, cell);
        }
        public CellWriter(Action<T, ExcelRange> write)
        {
            this.write = write;
        }
        public CellWriter(string propertyPath)
        {
            var p = Expression.Parameter(typeof(T));

        }
    }
    public class ObjectCellWriter<TConvert> : ICellWriter<object>
    {
        private readonly ICellWriter<TConvert> internalWriter;
        public ObjectCellWriter(ICellWriter<TConvert> internalWriter)
        {
            this.internalWriter = internalWriter;

        }

        public string Tag { get => internalWriter.Tag; }

        public int CellWidth { get => internalWriter.CellWidth; set => internalWriter.CellWidth = value; }
        public int CellHeight { get => internalWriter.CellHeight; set => internalWriter.CellHeight = value; }
        public Action<object, ExcelRange> PreWrite { get => internalWriter.PreWrite == null ? ((Action<object, ExcelRange>)null) : (o, c) => { internalWriter.PreWrite((TConvert)o, c); }; }
        public Action<object, ExcelRange> PostWrite { get => internalWriter.PostWrite == null ? ((Action<object, ExcelRange>)null) : (o, c) => { internalWriter.PostWrite((TConvert)o, c); }; }

        public void Write(object entity, ExcelRange cell)
        {
            internalWriter.Write((TConvert)entity, cell);
        }
    }
}