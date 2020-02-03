using BookShopApi.Data;
using BookShopApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services.ServicesImplementations
{
    public class BookService : IBookService
    {
        private readonly DataContext dataContext;

        public BookService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> CreateBookAsync(Book book)
        {
            await this.dataContext.Books.AddAsync(book);
            var created = await this.dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteBookByIdAsync(int BookId)
        {
            var book = await this.GetBookByIdAsync(BookId);

            if (book == null)
            {
                return false;
            }

            this.dataContext.Books.Remove(book);
            var deleted = this.dataContext.SaveChanges();
            return deleted > 0;
        }

        public async Task<Book> GetBookByIdAsync(int BookId)
        {
            return await this.dataContext.Books
                .Include(x => x.BookAuthors)
                .Include(x => x.BookCategories)
                .Include(x => x.Units)
                .SingleOrDefaultAsync(x => x.Id == BookId);
        }

        public async Task<bool> UpdateBookAsync(Book bookForUpdate)
        {
            this.dataContext.Books.Update(bookForUpdate);
            var updated = await this.dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var books = await this.dataContext.Books.ToListAsync();
            this.dataContext.Authors.Load();
            this.dataContext.Categories.Load();
            this.dataContext.Languages.Load();
            this.dataContext.BookAuthor.Load();
            this.dataContext.BookCategory.Load();
            return books;
            //.Include(x => x.BookAuthors)
            //.ThenInclude( bookauthors => bookauthors.Author)
            //.Include(x => x.BookCategories)
            //.ThenInclude(bookcategory => bookcategory.Category)
            //.Include(x => x.Units)
        }
    }
}
