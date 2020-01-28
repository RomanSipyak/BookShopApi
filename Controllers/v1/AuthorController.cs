using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Domain.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts;

namespace BookShopApi.Controllers.v1
{
    public class AuthorController : Controller
    {
        private readonly IAuthorService AuthorService;

        public AuthorController(IAuthorService authorService)
        {
            AuthorService = authorService;
        }

        [HttpPost(ApiRoutes.Authors.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorRequest createAuthorRequest)
        {
            var author = new Author
            {
                FullName = createAuthorRequest.FullName,
                Biography = createAuthorRequest.Biography
            };

            await AuthorService.CreateAuthorAsync(author);
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Authors.Get.Replace("{AuthorId}", author.Id.ToString());
            return Created(locationUrl, author);
        }

        [HttpGet(ApiRoutes.Authors.GetAll)]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            return Ok(await AuthorService.GetAuthorsAsync());
        }

        [HttpGet(ApiRoutes.Authors.Get)]
        public async Task<IActionResult> GetAuthorById([FromRoute] int AuthorId)
        {
            var author = await AuthorService.GetAuthorByIdAsync(AuthorId);

            if (author == null)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpPut(ApiRoutes.Authors.Update)]
        public async Task<IActionResult> UpdateAsync([FromRoute]int authorId, [FromBody] UpdateAuthorRequest request)
        {
            var author = await AuthorService.GetAuthorByIdAsync(authorId);
            author.FullName = request.FullName;
            author.Biography = request.Biography;
            var updated = await AuthorService.UpdateAuthorAsync(author);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(author);
        }

        [HttpDelete(ApiRoutes.Authors.Delete)]
        public async Task<IActionResult> DeleteAsync([FromRoute]int authorId)
        {
            var deletedAuthor = await AuthorService.DeleteAuthorByIdAsync(authorId);
            if (!deletedAuthor)
            {
                return NotFound();
            }
            return Ok(deletedAuthor);
        }
    }
}
