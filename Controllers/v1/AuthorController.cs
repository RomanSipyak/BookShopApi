using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Domain.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
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
        private readonly IAuthorService authorService;

        public AuthorController(IAuthorService authorService)
        {
            this.authorService = authorService;
        }

        [HttpPost(ApiRoutes.Authors.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateAuthorRequest createAuthorRequest)
        {
            var author = new Author
            {
                FullName = createAuthorRequest.FullName,
                Biography = createAuthorRequest.Biography,
            };

            await this.authorService.CreateAuthorAsync(author);
            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Authors.Get.Replace("{AuthorId}", author.Id.ToString());
            return this.Created(locationUrl, author);
        }

        [HttpGet(ApiRoutes.Authors.GetAll)]
        public async Task<IActionResult> GetAllAuthorsAsync()
        {
            return this.Ok(await this.authorService.GetAuthorsAsync());
        }

        [HttpGet(ApiRoutes.Authors.Get)]
        public async Task<IActionResult> GetAuthorById([FromRoute] int AuthorId)
        {
            var author = await this.authorService.GetAuthorByIdAsync(AuthorId);

            if (author == null)
            {
                return this.NotFound();
            }

            return this.Ok(author);
        }

        [HttpPut(ApiRoutes.Authors.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int authorId, [FromBody] UpdateAuthorRequest request)
        {
            var author = await this.authorService.GetAuthorByIdAsync(authorId);
            author.FullName = request.FullName;
            author.Biography = request.Biography;
            var updated = await this.authorService.UpdateAuthorAsync(author);
            if (!updated)
            {
                return this.NotFound();
            }

            return this.Ok(author);
        }

        [HttpDelete(ApiRoutes.Authors.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int authorId)
        {
            var deletedAuthor = await this.authorService.DeleteAuthorByIdAsync(authorId);
            if (!deletedAuthor)
            {
                return this.NotFound();
            }

            return this.Ok(deletedAuthor);
        }
    }
}
