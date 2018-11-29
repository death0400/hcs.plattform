using Hcs.Serialize.Xlsx;

namespace Hcs.Platform.File
{
    public interface IXlsxTEmplateFactory
    {
        IXlsxTemplate Get(string key);
    }
}