using api_screenvault.Dto;
using Azure.Storage;
using Azure.Storage.Blobs;

namespace api_screenvault.Services
{
    public class FileService
    {
        private readonly IConfiguration _config;
        private readonly BlobContainerClient _containerClient;
        private readonly string _storageAccount;
        private readonly string _storageKey;

        public FileService(IConfiguration config)
        {
            _config = config;
            _storageAccount = _config["BlobContainersStorageKey"];
            _storageKey = _config["BlobContainersStorageAccount"];
            var credential = new StorageSharedKeyCredential(_storageAccount, _storageKey);
            var blobUri = $"https://{_storageAccount}.blob.core.windows.net";
            var blobServiceClient = new BlobServiceClient(new Uri(blobUri), credential);
            _containerClient = blobServiceClient.GetBlobContainerClient("screenvault");
        }

        public async Task<List<BlobDto>> ListAsync()
        {
            List<BlobDto> files = new List<BlobDto>();

            await foreach (var file in _containerClient.GetBlobsAsync())
            {
                string uri = _containerClient.Uri.ToString();
                var name = file.Name;
                var fullUri = $"{uri}/{name}";

                files.Add(
                    new BlobDto
                    {
                        Uri = fullUri,
                        Name = name,
                        ContentType = file.Properties.ContentType
                    });
            }
            return files;
        }
    }
}