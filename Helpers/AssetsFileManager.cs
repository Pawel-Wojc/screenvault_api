using api_screenvault.Model;
using System.Linq.Expressions;

namespace api_screenvault.Helpers
{
    public interface IAssetsFileManager
    {

        Task<Post> SaveFile(IFormFile fileFromUser);
    }
    public class AssetsFileManager : IAssetsFileManager
    {
        
        private readonly IConfiguration _config;
        private readonly string _assetsFolderPath;
        public AssetsFileManager(IConfiguration config)
        {
            _config = config;
            _assetsFolderPath = _config["AssetsFolderPath"];
        }

        public async Task<Post> SaveFile(IFormFile fileFromUser)
        {
            if (fileFromUser == null || fileFromUser.Length == 0) { 
             throw new ArgumentNullException(nameof(fileFromUser));
            }

            Post post = new Post() { Id = Guid.NewGuid() };
            var filePath = Path.Combine(_assetsFolderPath, post.Id.ToString());

            if (!Directory.Exists(_assetsFolderPath))
            {
                Directory.CreateDirectory(_assetsFolderPath);
            }
            try
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await fileFromUser.CopyToAsync(stream);  // Copy the content of the uploaded file to the FileStream
                }
            }
            catch (Exception ex) {
                throw new Exception($"Error saving the file: {ex.Message}", ex);
            }
            post.Path = filePath;
            return post;

            
        }

        public async Task<FileStream> ReadFile(Guid Id) {
            var filePath = Path.Combine(_assetsFolderPath, Id.ToString());
            
            if (File.Exists(filePath))
            {
                return new FileStream(filePath, FileMode.Open, FileAccess.Read); // Return a FileStream
            }
            else
            {
                throw new FileNotFoundException($"File with ID {Id} not found in Assets folder.");
            }
        }

        public bool DeleteFile(Guid Id) {
            return false;
        }       
    }
}
