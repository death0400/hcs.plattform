using System;
using System.Collections.Generic;
using System.Linq;

namespace Hcs.Platform.Flow
{
    public class ExportSettings<T>
    {
        internal Func<Hcs.Serialize.Xlsx.IXlsxTemplate, Hcs.Serialize.Xlsx.IXlsxTemplate> MapTemplate { get; set; } = x => x;
        public void OverrideTemplate(Func<Hcs.Serialize.Xlsx.IXlsxTemplate, Hcs.Serialize.Xlsx.IXlsxTemplate> map)
        {
            var o = MapTemplate;
            MapTemplate = x => map(o(x));
        }
        internal Action<Hcs.Serialize.Xlsx.IPropertyCellWriter[]> ConfigCellWriter { get; set; } = x => { };
        public void ConfigCellWriters(Action<Hcs.Serialize.Xlsx.IPropertyCellWriter[]> map)
        {
            var o = ConfigCellWriter;
            ConfigCellWriter = x =>
            {
                o(x);
                map(x);
            };
        }
        internal Func<Hcs.Serialize.Xlsx.IXlsxProcess<IEnumerable<T>>[], Hcs.Serialize.Xlsx.IXlsxProcess<IEnumerable<T>>[]> MapProcess { get; set; } = x => x;
        public void OverrideProcesses(Func<Hcs.Serialize.Xlsx.IXlsxProcess<IEnumerable<T>>[], Hcs.Serialize.Xlsx.IXlsxProcess<IEnumerable<T>>[]> map)
        {
            var o = MapProcess;
            MapProcess = x => map(o(x));
        }
    }
    public class ExportColumn
    {
        public string PropertyName { get; set; }
        public string DisplayName { get; set; }
        static System.Text.RegularExpressions.Regex validator = new System.Text.RegularExpressions.Regex(@"^([^/,]+/[^/,]+,)*[^/,]+/[^/,]+$", System.Text.RegularExpressions.RegexOptions.Compiled);
        public static ExportColumn[] Parse(string input)
        {
            if (input == null)
            {
                throw new ArgumentNullException(nameof(input));
            }
            input = input.Trim();
            if (!validator.IsMatch(input))
            {
                throw new ArgumentException($"value {input} is not correct format", nameof(input));
            }
            return input.Split(',').Select(x =>
             {
                 var n = x.Split('/');
                 return new ExportColumn
                 {
                     PropertyName = n[0],
                     DisplayName = n[1]
                 };
             }).ToArray();
        }
    }

}