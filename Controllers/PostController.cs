using api_screenvault.Data;
using api_screenvault.Model;
using api_screenvault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Reflection.Metadata;

namespace api_screenvault.Controllers
{
    [Route("post/")]
    [ApiController]
    [AllowAnonymous]
    public class PostController : Controller
    {
        private ApplicationDbContext _context;
        private FileService _fileService;
        public PostController(ApplicationDbContext context, FileService fileService)

        {
            _fileService = fileService;
            _context = context;
        }


        //[HttpGet]
        //public IActionResult GetTwentyPublicPosts(string lastPostId) {
        //    return Ok();

        //}

        [HttpPost]
        [Route("CreateAnonymousPost")]
        public async Task<IActionResult> CreateAnonymousPost(string fileName, IFormFile file)
        {

            if (file == null || file.Length == 0) {
                return BadRequest("No file uploaded");
            }

            

            var result = await _fileService.UploadAsyn(fileName, file);
               return Ok(result);

            
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(string filename) {
            var result = await _fileService.DownloadAsync(filename);
            return File(result.Content, result.ContentType, result.Name);
        }

        [HttpGet]
        [Route("ListAllFiles")]
        public async Task<IActionResult> ListAllFiles() {
            var result = await _fileService.ListAsync();

            return Ok(result);
        }


        //[HttpPost]
        //public async Task<IActionResult> Upload(IFormFile file) {
        //    var result = await _fileService.UploadAsyn(file);
        //    return Ok(result);
        //}

        [HttpDelete]
        [Route("Delete")]
        public async Task<IActionResult> Delete(string filename) { 
            var result = await _fileService.DeleteAsync(filename);
            return Ok(result);
        }
    }
}
