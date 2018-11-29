using Hcs.Platform.File;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using System;
using System.IO;
using System.Threading.Tasks;
namespace Hcs.Platform.Core.File.AzureBlob
{
    public class AzureBlobStorage : IFileStorage
    {
        CloudBlobContainer container;
        public AzureBlobStorage(string storageAccountName, string accessKey, string containerName)
        {
            var storageAccount = new CloudStorageAccount(
                   new Microsoft.WindowsAzure.Storage.Auth.StorageCredentials(
                   storageAccountName,
                   accessKey), true);
            var blobClient = storageAccount.CreateCloudBlobClient();
            container = blobClient.GetContainerReference(containerName);
        }
        public async Task<string> Create(Stream stream)
        {
            string key;
            CloudBlockBlob blob = null;
            do
            {
                key = Guid.NewGuid().ToString("n");
                blob = container.GetBlockBlobReference(key);
            } while (await blob.ExistsAsync());
            await blob.UploadFromStreamAsync(stream);
            return key;
        }

        public async Task Delete(string key)
        {
            var blob = container.GetBlockBlobReference(key);
            await blob.DeleteIfExistsAsync();
        }

        public async Task<bool> Exists(string key)
        {
            return await container.GetBlockBlobReference(key).ExistsAsync();
        }

        public async Task<Stream> Open(string key)
        {
            var blob = container.GetBlockBlobReference(key);
            if (await blob.ExistsAsync())
            {
                return await blob.OpenReadAsync();
            }
            else
            {
                return null;
            }
        }

        public async Task Update(string key, Stream stream)
        {
            var blob = container.GetBlockBlobReference(key);
            if (await blob.ExistsAsync())
            {
                await blob.UploadFromStreamAsync(stream);
            }
        }
    }
}