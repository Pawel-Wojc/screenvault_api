using api_screenvault.Data;
using api_screenvault.Dto;
using api_screenvault.Helpers;
using api_screenvault.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace api_screenvault.Services
{
    public interface IAnonymousPostService {
        Task<AnonymousPostCreateResponseDto> CreateAnonymousPost(IFormFile file, string title);
        Task<AnonymousPostGetResponseDto> GetAnonymousPost(string linkId);


    }
    public class AnonymousPostService : IAnonymousPostService
    {
        private readonly IAnonymousAzureBlobHandling _anonymousAzureBlobHandling;
        private readonly ISharedPostIdGenerator _sharedPostIdGenerator;
        private readonly AssetsFileManager _assetsFileManager;
        private readonly ApplicationDbContext _dbContext;
        public AnonymousPostService(IAnonymousAzureBlobHandling anonymousAzureBlobHandling, ISharedPostIdGenerator sharedPostIdGenerator, AssetsFileManager assetsFileManager, ApplicationDbContext dbContext)
        {
            _anonymousAzureBlobHandling = anonymousAzureBlobHandling;
            _sharedPostIdGenerator = sharedPostIdGenerator;
            _assetsFileManager = assetsFileManager;
            _dbContext = dbContext;
        }

        public async Task<AnonymousPostCreateResponseDto> CreateAnonymousPost(IFormFile file, string title) {

           // BlobResponseDto  blobResponse = await _anonymousAzureBlobHandling.UploadAnonymousAsyn(file);
           
            Post post = await _assetsFileManager.SaveFile(file);

            post.Title = title;
            post.LinkId = _sharedPostIdGenerator.GenerateUniqueLinkId();

            return SaveAnonymousPostToDb(post);
        }

        public async Task<AnonymousPostGetResponseDto> GetAnonymousPost(string linkId) {
            AnonymousPostGetResponseDto anonymousPost = new();
            Post? post = _dbContext.Posts.FirstOrDefault(post => post.LinkId == linkId);
            if (post == null) {
                return new AnonymousPostGetResponseDto
                {
                    Id = new Guid(),
                    LinkId = linkId,
                    Error = true,
                    ErrorMessage = $"Post with id:{linkId} not found",

                };
            }
            FileStream? file = null;
            try
            {
                 file = await _assetsFileManager.ReadFile(post.Id);

            }
            catch (Exception e) {
                //if we find a post with this id but not file this means there is divergence between db structure and file structure
                return new AnonymousPostGetResponseDto
                {
                    Id = new Guid(),
                    LinkId = linkId,
                    Error = true,
                    ErrorMessage = e.Message,
                };
            }
            
            return new AnonymousPostGetResponseDto
            {
                Id = post.Id,
                Title = post.Title,
                LinkId = post.LinkId,
                Error = false,
                ErrorMessage = null,
                File = file
            }; ;
        }

        private AnonymousPostCreateResponseDto SaveAnonymousPostToDb(Post post) {
            try
            {
                _dbContext.Posts.Add(post);
                _dbContext.SaveChanges();
                return new AnonymousPostCreateResponseDto
                {
                    Id= post.Id,
                    LinkId= post.LinkId,
                    Error = false,
                    ErrorMessage = null
                };
            }
            catch (Exception ex) {
                _assetsFileManager.DeleteFile(post.Id);
                return new AnonymousPostCreateResponseDto
                {
                    Id= post.Id,
                    LinkId= post.LinkId,
                    Error = true,
                    ErrorMessage = ex.Message

                };
            }
        }      
    }
}
