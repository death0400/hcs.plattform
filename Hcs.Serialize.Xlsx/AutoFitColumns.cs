using System.Linq;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class AutoFitColumns<T> : IXlsxProcess<T>
    {
        private readonly int? sheetIndex;

        public AutoFitColumns(int? sheetIndex = null)
        {
            this.sheetIndex = sheetIndex;
        }
        public void Run(ExcelPackage package, T data)
        {
            var sheets = package.Workbook.Worksheets.AsEnumerable();
            if (sheetIndex.HasValue)
            {
                sheets = sheets.Where(x => x.Index == sheetIndex.Value);
            }
            foreach (var s in sheets)
            {
                s.Cells[s.Dimension.Address].AutoFitColumns();
            }
        }
    }
}