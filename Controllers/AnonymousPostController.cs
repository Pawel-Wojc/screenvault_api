using api_screenvault.Data;
using api_screenvault.Dto;
using api_screenvault.Model;
using api_screenvault.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query.Internal;
using Microsoft.EntityFrameworkCore.SqlServer.Query.Internal;
using System.Diagnostics.CodeAnalysis;
using System.Net.Http.Headers;
using System.Reflection.Metadata;

namespace api_screenvault.Controllers
{
    [Route("post/anonymous/")]
    [ApiController]
    public class AnonymousPostController(ApplicationDbContext context, IAnonymousPostService anonymousPostService) : Controller
    {
        private readonly ApplicationDbContext _context = context;
        private readonly IAnonymousPostService _anonymousPostService = anonymousPostService;


        [HttpPost]
        [Route("create")]
        public async Task<IActionResult> CreateAnonymousPost(IFormFile file, string title)
        {

            var result = await _anonymousPostService.CreateAnonymousPost(file, title);

            if (result.Error) { 
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result);


        }

        [HttpGet]
        [Route("get")]
        public async  Task<IActionResult> GetAnonymousPostByLinkId(string linkId) {
           var post = await _anonymousPostService.GetAnonymousPost(linkId);

            if (post.Error) {
                if (post.ErrorMessage.Contains("not found"))
                {
                    return NotFound(post.ErrorMessage);
                }
                return BadRequest(post.ErrorMessage);
            }

            return new FileStreamResult(post.File, "application/octet-stream") { };
          
        }
    }
}