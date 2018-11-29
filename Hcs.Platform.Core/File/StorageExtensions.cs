using Microsoft.Extensions.DependencyInjection;

namespace Hcs.Platform
{
    public static class StorageExtensions
    {
        public static PlatformBuilder UseLocalFileStroage(this PlatformBuilder builder, string rootPath)
        {
            builder.Services.AddSingleton<File.IFileStorage>(c => new Core.File.LocalDisk.LocalDiskFileStorage(rootPath));
            return builder;
        }
        public static PlatformBuilder UseAzureBlobStorage(this PlatformBuilder builder, string storageAccountName, string accessKey, string containerName)
        {
            builder.Services.AddSingleton<File.IFileStorage>(c => new Core.File.AzureBlob.AzureBlobStorage(storageAccountName, accessKey, containerName));
            return builder;
        }
    }
}