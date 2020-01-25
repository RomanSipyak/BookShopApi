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
    public class BookController : Controller
    {
        private readonly IBookService BookService;
        private readonly ILanguageService LanguageService;


        public BookController(IBookService bookService,ILanguageService languageService)
        {
            BookService = bookService;
            LanguageService = languageService;
        }

        [HttpPost(ApiRoutes.Book.Create)]
        public async Task<IActionResult> CreateBookAsync([FromBody] CreateBookRequest createBookRequest)
        {
            var book = new Book();
            book.Description = createBookRequest.Description;
            book.Language = await LanguageService.GetLanguageByTitleAsync(createBookRequest.Language);
            book.BookAuthors = createBookRequest.BookAuthors.Select(x => new BookAuthor { AuthorId = x, BookId = book.Id }).ToList();
            book.BookCategories = createBookRequest.BookCategories.Select(x => new BookCategory { CategoryId = x, BookId = book.Id }).ToList();

            await BookService.CreateBookAsync(book);

            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Book.Get.Replace("{BookId}", book.Id.ToString());

            return Created(locationUrl, book);
        }
    }
}
