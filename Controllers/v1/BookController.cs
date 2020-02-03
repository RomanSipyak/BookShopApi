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
    public class BookController : Controller
    {
        private readonly IBookService bookService;
        private readonly ILanguageService languageService;
        private readonly ICategoryService categoryService;
        private readonly IAuthorService authorService;

        public BookController(IBookService bookService, ILanguageService languageService, IAuthorService authorService, ICategoryService categoryService)
        {
            this.bookService = bookService;
            this.languageService = languageService;
            this.authorService = authorService;
            this.categoryService = categoryService;
        }

        [HttpPost(ApiRoutes.Books.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookRequest createBookRequest)
        {
            var book = new Book();
            book.Title = createBookRequest.Title;
            book.Description = createBookRequest.Description;
            book.Price = Convert.ToDecimal(createBookRequest.Price, new System.Globalization.CultureInfo("en-US"));
            book.Language = await this.languageService.GetLanguageByTitleAsync(createBookRequest.Language.Title);
            book.BookAuthors = createBookRequest.BookAuthors.Select(x => new BookAuthor { Author = this.authorService.GetAuthorById(x.Id), Book = book }).ToList();
            book.BookCategories = createBookRequest.BookCategories.Select(x => new BookCategory { Category = this.categoryService.GetCategoryById(x.Id), Book = book }).ToList();

            await this.bookService.CreateBookAsync(book);

            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Books.Get.Replace("{BookId}", book.Id.ToString());

            return this.Created(locationUrl, book);
        }

        [HttpGet(ApiRoutes.Books.GetAll)]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            return this.Ok(await this.bookService.GetBooksAsync());
        }

        [HttpGet(ApiRoutes.Books.Get)]
        public async Task<IActionResult> GetBookById([FromRoute] int BookId)
        {
            var bookDeleted = await bookService.GetBookByIdAsync(BookId);
            if (bookDeleted == null)
            {
                return NotFound();
            }

            return Ok(bookDeleted);
        }


        [HttpPut(ApiRoutes.Books.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int BookId, [FromBody] UpdateBookRequest updateBookRequest)
        {
            var book = await this.bookService.GetBookByIdAsync(BookId);

            book.Title = updateBookRequest.Title;
            book.Description = updateBookRequest.Description;
            book.Price = Convert.ToDecimal(updateBookRequest.Price, new System.Globalization.CultureInfo("en-US"));
            book.Language = await this.languageService.GetLanguageByTitleAsync(updateBookRequest.Language.Title);
            book.BookAuthors = updateBookRequest.BookAuthors.Select(x => new BookAuthor { Author = this.authorService.GetAuthorById(x.Id), Book = book }).ToList();
            book.BookCategories = updateBookRequest.BookCategories.Select(x => new BookCategory { Category = this.categoryService.GetCategoryById(x.Id), Book = book }).ToList();

            var updated = await this.bookService.UpdateBookAsync(book);
            if (!updated)
            {
                return this.NotFound();
            }

            return this.Ok(book);
        }

        [HttpDelete(ApiRoutes.Books.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int bookid)
        {
            var bookDeleted = await this.bookService.DeleteBookByIdAsync(bookid);
            if (!bookDeleted)
            {
                return this.NotFound();
            }

            return this.Ok(bookDeleted);
        }
    }
}
