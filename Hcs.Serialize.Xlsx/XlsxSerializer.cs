using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;
using System;
using OfficeOpenXml;

namespace Hcs.Serialize.Xlsx
{
    public class XlsxSerializer<T>
    {
        private readonly XlsxSerializeSettings<T> settings;
        public static XlsxSerializer<IEnumerable<TElement>> Create<TElement>(IEnumerable<TElement> data)
        {
            var properties = typeof(TElement).GetProperties();
            var ps = properties.Select(x => new Hcs.Expressions.PropertyAccessor(typeof(TElement), x.Name)).ToArray();
            var header = new WriteXlsxRow<TElement>(0, 0, 0, properties.Select(x => new CellWriter<TElement>((e, cell) => cell.Value = x.Name)).ToArray());
            var body = new WriteXlsxRow<TElement>(0, 1, 0, ps.Select(x => new CellWriter<TElement>((e, c) => c.Value = x.Getter(e))).ToArray());
            return new XlsxSerializer<IEnumerable<TElement>>(new XlsxSerializeSettings<IEnumerable<TElement>>(header, body));
        }
        public static XlsxSerializer<IEnumerable<object>> Create(Type elementType, IEnumerable<object> data)
        {
            var properties = elementType.GetProperties();
            var ps = properties.Select(x => new Hcs.Expressions.PropertyAccessor(elementType, x.Name)).ToArray();
            var header = new WriteXlsxRow<object>(0, 0, 0, properties.Select(x => new CellWriter<object>((e, cell) => cell.Value = x.Name)).ToArray());
            var body = new WriteXlsxRow<object>(0, 1, 0, ps.Select(x => new CellWriter<object>((e, c) => c.Value = x.Getter(e))).ToArray());
            return new XlsxSerializer<IEnumerable<object>>(new XlsxSerializeSettings<IEnumerable<object>>(header, body));
        }
        public XlsxSerializer(XlsxSerializeSettings<T> settings)
        {
            this.settings = settings;
        }
        public System.IO.Stream Serialize(T data)
        {
            var ms = new System.IO.MemoryStream();
            using (var pkg = new OfficeOpenXml.ExcelPackage(ms))
            {
                if (settings.Template != null)
                {
                    using (var tmp = new OfficeOpenXml.ExcelPackage(settings.Template.Open()))
                    {
                        foreach (var sheet in tmp.Workbook.Worksheets)
                        {
                            pkg.Workbook.Worksheets.Add(sheet.Name, sheet);
                        }
                    }
                }
                if (!pkg.Workbook.Worksheets.Any())
                {
                    pkg.Workbook.Worksheets.Add("Export");
                }
                foreach (var p in settings.Processes)
                {
                    p.Run(pkg, data);
                }
                pkg.Save();
            }
            ms.Position = 0;
            return ms;
        }
    }
}