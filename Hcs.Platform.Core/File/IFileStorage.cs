using System.IO;
using System.Threading.Tasks;

namespace Hcs.Platform.File
{
    public interface IFileStorage
    {
        Task<string> Create(Stream stream);
        Task Update(string key, Stream stream);
        Task Delete(string key);
        Task<Stream> Open(string key);
        Task<bool> Exists(string key);
    }
}