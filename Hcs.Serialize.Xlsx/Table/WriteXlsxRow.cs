using System;
using System.Collections.Generic;
using System.Linq;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class ObjectWriteXlsxRow<TConvert> : WriteXlsxRow<object>
    {
        public ObjectWriteXlsxRow(int sheetIndex, int startRow, int startCol, params ICellWriter<TConvert>[] cellWriters) : base(sheetIndex, startRow, startCol, cellWriters.Select(x => new ObjectCellWriter<TConvert>(x)).ToArray())
        {

        }
    }
    public class WriteXlsxRow<T, TElement> : IXlsxProcess<T>
    {
        private readonly Func<T, IEnumerable<TElement>> select;
        private WriteXlsxRow<TElement> rowWriter;

        public WriteXlsxRow(int sheetIndex, int startRow, int startCol, Func<T, IEnumerable<TElement>> select, params ICellWriter<TElement>[] cellWriters)
        {
            rowWriter = new WriteXlsxRow<TElement>(sheetIndex, startRow, startCol, cellWriters);
            this.select = select;
        }
        public void Run(ExcelPackage package, T data)
        {
            rowWriter.Run(package, select(data));
        }
    }
    public class WriteXlsxRow<T> : IXlsxProcess<IEnumerable<T>>
    {
        private readonly int sheetIndex;
        private readonly int startRow;
        private readonly int startCol;
        private readonly ICellWriter<T>[] cellWriters;
        public WriteXlsxRow(int sheetIndex, int startRow, int startCol, params ICellWriter<T>[] cellWriters)
        {
            this.sheetIndex = sheetIndex;
            this.startRow = startRow + 1;
            this.startCol = startCol + 1;
            this.cellWriters = cellWriters;
        }
        public void Run(ExcelPackage package, IEnumerable<T> data)
        {
            var sheet = package.Workbook.Worksheets[sheetIndex];
            int currentRow = startRow;
            int currentCol = startCol;
            foreach (var d in data)
            {
                int r = 0;
                int c = 0;
                foreach (var cw in cellWriters)
                {
                    var sr = currentRow;
                    var sc = currentCol + c;
                    var er = currentRow + cw.CellHeight - 1;
                    var ec = currentCol + c + cw.CellWidth - 1;
                    var cell = sheet.Cells[sr, sc, er, ec];
                    if (er - sr > 0 || ec - sc > 0)
                    {
                        cell.Merge = true;
                    }
                    try
                    {
                        cw.PreWrite?.Invoke(d, cell);
                        cw.Write(d, cell);
                        cw.PostWrite?.Invoke(d, cell);
                    }
                    catch (Exception ex)
                    {
                        throw new CellWriteException<T>(cw, ex);
                    }
                    r = Math.Max(r, cw.CellHeight);
                    c += cw.CellWidth;
                }
                currentRow += r;
            }
        }
    }
}