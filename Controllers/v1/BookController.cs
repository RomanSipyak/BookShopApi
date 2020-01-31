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
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class BookController : Controller
    {
        private readonly IBookService BookService;
        private readonly ILanguageService LanguageService;
        private readonly ICategoryService CategoryService;
        private readonly IAuthorService AuthorService;

        public BookController(IBookService bookService, ILanguageService languageService, IAuthorService authorService, ICategoryService categoryService)
        {
            BookService = bookService;
            LanguageService = languageService;
            AuthorService = authorService;
            CategoryService = categoryService;
        }
        
        [HttpPost(ApiRoutes.Books.Create)]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookRequest createBookRequest)
        {
            var book = new Book();
            book.Title = createBookRequest.Title;
            book.Description = createBookRequest.Description;
            book.Language = await LanguageService.GetLanguageByTitleAsync(createBookRequest.Language.Title);
            book.BookAuthors =  createBookRequest.BookAuthors.Select(x => new BookAuthor { Author = AuthorService.GetAuthorById(x.Id), Book = book }).ToList();
            book.BookCategories = createBookRequest.BookCategories.Select(x => new BookCategory { Category = CategoryService.GetCategoryById(x.Id), Book = book }).ToList();

            await BookService.CreateBookAsync(book);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Books.Get.Replace("{BookId}", book.Id.ToString());

            return Created(locationUrl, book);
        }
       
        [HttpGet(ApiRoutes.Books.GetAll)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<IActionResult> GetAllBooksAsync()
        {
            return Ok(await BookService.GetBooksAsync());
        }

        [HttpGet(ApiRoutes.Books.Get)]
        public async Task<IActionResult> GetBookById([FromRoute] int BookId)
        {
            var bookDeleted = await BookService.GetBookByIdAsync(BookId);
            if (bookDeleted == null)
            {
                return NotFound();
            }

            return Ok(bookDeleted);
        }


        [HttpPut(ApiRoutes.Books.Update)]
        public async Task<IActionResult> UpdateAsync([FromRoute]int bookId, [FromBody] UpdateBookRequest updateBookRequest)
        {
            var book = await BookService.GetBookByIdAsync(bookId);

            book.Title = updateBookRequest.Title;
            book.Description = updateBookRequest.Description;
            book.Language = await LanguageService.GetLanguageByTitleAsync(updateBookRequest.Language.Title);
            book.BookAuthors = updateBookRequest.BookAuthors.Select(x => new BookAuthor { Author = AuthorService.GetAuthorById(x.Id), Book = book }).ToList();
            book.BookCategories = updateBookRequest.BookCategories.Select(x => new BookCategory { Category = CategoryService.GetCategoryById(x.Id), Book = book }).ToList();
           
            var updated = await BookService.UpdateBookAsync(book);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(book);
        }

        [HttpDelete(ApiRoutes.Books.Delete)]
        public async Task<IActionResult> DeleteAsync([FromRoute]int bookid)
        {
            var bookDeleted = await BookService.DeleteBookByIdAsync(bookid);
            if (!bookDeleted)
            {
                return NotFound();
            }
            return Ok(bookDeleted);
        }
    }
}
