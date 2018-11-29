using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class WriteXlsxConst<T> : IXlsxProcess<T>
    {
        private readonly int sheetIndex;
        private readonly int startRow;
        private readonly int startCol;
        private readonly object[] constData;
        public Action<ExcelRange> PostSetValue { get; set; }
        public WriteXlsxConst(int sheetIndex, int startRow, int startCol, params object[] data)
        {
            this.constData = data;
            this.sheetIndex = sheetIndex;
            this.startRow = startRow + 1;
            this.startCol = startCol + 1;
        }
        public void Run(ExcelPackage package, T data)
        {
            var sheet = package.Workbook.Worksheets[sheetIndex];
            for (int i = 0, c = constData.Length; i < c; i++)
            {
                var cell = sheet.Cells[startRow, startCol + i];
                cell.Value = constData[i];
                PostSetValue?.Invoke(cell);
            }
        }
    }
}