using api_screenvault.Services;
using Azure.Storage.Blobs;
using Azure.Storage;
using api_screenvault.Dto;
using Azure.Storage.Blobs.Models;
using Azure;

namespace api_screenvault.Helpers

{
    public interface IAnonymousAzureBlobHandling {
        public Task<BlobResponseDto> UploadAnonymousAsync(IFormFile fileFromUser);


    }
    public class AnonymousAzureBlobHandling : IAnonymousAzureBlobHandling
    {
        
        private readonly IConfiguration _config;
        private readonly ILogger _logger;
        private readonly BlobContainerClient _containerClient;
        private readonly string _storageAccount;
        private readonly string _storageKey;

        public AnonymousAzureBlobHandling(IConfiguration config, ILogger<AnonymousAzureBlobHandling> logger)
        {
            _logger = logger;
            _config = config;
            _storageAccount = _config["PublicBlobContainersStorageAccount"];
            _storageKey = _config["PublicBlobContainersStorageKey"];
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

        public async Task<BlobResponseDto> UploadAnonymousAsync(IFormFile fileFromUser)
        {
            BlobResponseDto blobResponse = new();
            if (!ExtensionChecker.ExtensionIsValid(fileFromUser.FileName)) {
                throw new Exception("Invalid extension");
            }
            
            var pathOnBlobStorage = $"{fileFromUser.FileName}.{Guid.NewGuid().ToString()}" ; // garantee that the filename is unique

            BlobClient blobClient = _containerClient.GetBlobClient(pathOnBlobStorage);
            Response<BlobContentInfo> resultFromBlobStorage;

            await using (Stream? data = fileFromUser.OpenReadStream())
            {
                try
                {
                    resultFromBlobStorage = await blobClient.UploadAsync(data);
                }
                catch (Exception e)
                {
                    if (e.InnerException is RequestFailedException)
                    {
                        _logger.LogWarning($"Failed to upload file, Error: {e.Message}");
                        blobResponse.Status = "Failed to upload file";
                        blobResponse.Error = true;
                        blobResponse.Blob.Uri = null;
                        blobResponse.Blob.Name = null;
                        return blobResponse;
                    }

                    blobResponse.Status = "Something went wrong";
                    blobResponse.Error = true;
                    blobResponse.Blob.Uri = null;
                    blobResponse.Blob.Name = null;
                    return blobResponse;
                }
            }

            blobResponse.Status = $"File {fileFromUser.FileName} Uploaded Successfully";
            blobResponse.Error = false;
            blobResponse.Blob.Uri = blobClient.Uri.AbsoluteUri;
            blobResponse.Blob.Name = blobClient.Name;
            

            return  blobResponse;
        }


        

        public async Task<BlobDto?> DownloadAsync(string blobFilename)
        {
            BlobClient file = _containerClient.GetBlobClient(blobFilename);

            if (await file.ExistsAsync())
            {
                var data = await file.OpenReadAsync();
                Stream blobContent = data;

                var content = await file.DownloadContentAsync();

                string name = blobFilename;

                string contentType = content.Value.Details.ContentType;

                return new BlobDto { Content = blobContent, Name = name, ContentType = contentType };
            }

            return null;
        }


        public async Task<BlobResponseDto> DeleteAsync(string blobFileName)
        {
            BlobClient file = _containerClient.GetBlobClient(blobFileName);

            await file.DeleteAsync();

            return new BlobResponseDto { Error = false, Status = $"File: {blobFileName} has been succesfully deleted" };
        }
    }
    
}
