using System.IO;

namespace Hcs.Serialize.Xlsx.Template
{
    public class FileXlsxTemplate : IXlsxTemplate
    {
        private readonly string fileName;
        public FileXlsxTemplate(string fileName)
        {
            this.fileName = fileName;
            if (fileName == null)
            {
                throw new System.ArgumentNullException(nameof(fileName));
            }
        }
        public Stream Open()
        {
            return System.IO.File.Open(fileName, FileMode.Open, FileAccess.Read, FileShare.Read);
        }
    }
}