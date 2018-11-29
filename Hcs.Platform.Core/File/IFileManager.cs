using System;
using System.IO;
using System.Threading.Tasks;

namespace Hcs.Platform.File
{
    public interface IFileManager
    {
        Task<string> Create(Stream stream, string name, string dir, string mimeType);
        Task<IPlatformFileInfo> GetFileInfo(string key);
        Task UpdateContent(string key, Stream stream);
        Task<Stream> Open(string key);
        Task Rename(string name, string key);
        Task Move(string key, string newDir);
        Task Delete(string key);
        Task ConfirmFile(string key);
        Task ClearUnConfirmed();
    }
}