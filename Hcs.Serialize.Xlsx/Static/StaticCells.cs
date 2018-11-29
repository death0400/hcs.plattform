using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx.Static
{
    public class StaticCells<T> : IXlsxProcess<T>
    {
        StaticCell<T>[] cellWriters;
        public StaticCells(params StaticCell<T>[] cells)
        {
            cellWriters = cells;
        }
        public void Run(ExcelPackage package, T data)
        {
            foreach (var c in cellWriters)
            {
                var cell = package.Workbook.Worksheets[c.SheetIndex].Cells[c.Address];
                var cw = c.CellWriter;
                try
                {
                    cw.PreWrite?.Invoke(data, cell);
                    cw.Write(data, cell);
                    cw.PostWrite?.Invoke(data, cell);
                }
                catch (Exception ex)
                {
                    throw new CellWriteException<T>(cw, ex);
                }
            }
        }
    }
}