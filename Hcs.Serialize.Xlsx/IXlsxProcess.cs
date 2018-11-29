using System.Threading.Tasks;

namespace Hcs.Serialize.Xlsx
{
    public interface IXlsxProcess<in T>
    {
        void Run(OfficeOpenXml.ExcelPackage package, T data);
    }
}