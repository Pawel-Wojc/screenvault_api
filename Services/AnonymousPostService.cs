using api_screenvault.Dto;
using api_screenvault.Helpers;
using api_screenvault.Model;
using Microsoft.AspNetCore.Http.HttpResults;

namespace api_screenvault.Services
{
    public interface IAnonymousPostService {
        Task<string> CreateAnonymousPost(IFormFile file, string title);


    }
    public class AnonymousPostService : IAnonymousPostService
    {
        private readonly IAnonymousAzureBlobHandling _anonymousAzureBlobHandling;
        private readonly ISharedPostIdGenerator _sharedPostIdGenerator;
        public AnonymousPostService(IAnonymousAzureBlobHandling anonymousAzureBlobHandling, ISharedPostIdGenerator sharedPostIdGenerator)
        {
            _anonymousAzureBlobHandling = anonymousAzureBlobHandling;
            _sharedPostIdGenerator = sharedPostIdGenerator;
        }

        public async Task<string> CreateAnonymousPost(IFormFile file, string title) {

            BlobResponseDto  blobResponse = await _anonymousAzureBlobHandling.UploadAnonymousAsyn(file);
            if (blobResponse.Error) {
                return "cant create"+ blobResponse.Status; 
            }

            Post post = new() { Id = new Guid(), Title = title, IsAnonymous = true };

            post.LinkId = _sharedPostIdGenerator.GenerateUniqueLinkId();
            post.Uri = blobResponse.Blob.Uri;
            return post.LinkId;

        }

       
    }
}
