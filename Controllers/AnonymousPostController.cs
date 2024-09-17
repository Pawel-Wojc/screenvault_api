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
    [Route("post/anonymous/")]
    [ApiController]
    public class AnonymousPostController(ApplicationDbContext context, FileService fileService, IAnonymousPostService anonymousPostService) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly FileService _fileService = fileService;
        private readonly IAnonymousPostService _anonymousPostService = anonymousPostService;


        //[HttpGet]
        //public IActionResult GetTwentyPublicPosts(string lastPostId) {
        //    return Ok();

        //}

        [HttpPost]
        [Route("CreateAnonymousPost")]
        public async Task<IActionResult> CreateAnonymousPost(IFormFile file, string title)
        {

            var result = await _anonymousPostService.CreateAnonymousPost(file, title);
            return Ok(result);



            //var result = await _fileService.UploadAsyn(file);
            //if (result.Error)
            //{
            //    return BadRequest(result);
            //}
            //return Ok(result);
        }

        [HttpGet]
        [Route("Download")]
        public async Task<IActionResult> Download(string filename)
        {
            var result = await _fileService.DownloadAsync(filename);
            return File(result.Content, result.ContentType, result.Name);
        }

        [HttpGet]
        [Route("ListAllFiles")]
        public async Task<IActionResult> ListAllFiles()
        {
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
        public async Task<IActionResult> Delete(string filename)
        {
            var result = await _fileService.DeleteAsync(filename);
            return Ok(result);
        }
    }
}