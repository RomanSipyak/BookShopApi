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
        private readonly DataContext DataContext;

        public BookService(DataContext dataContext)
        {
            this.DataContext = dataContext;
        }

        public async Task<bool> CreateBookAsync(Book book)
        {
            await DataContext.Books.AddAsync(book);
            var created = await DataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteBookByIdAsync(int BookId)
        {
            var book = await GetBookByIdAsync(BookId);

            if (book == null)
            {
                return false;
            }
            DataContext.Books.Remove(book);
            var deleted = DataContext.SaveChanges();
            return deleted > 0;
        }

        public async Task<Book> GetBookByIdAsync(int BookId)
        {
            return await DataContext.Books
                .Include(x => x.BookAuthors)
                .Include(x => x.BookCategories)
                .Include(x => x.Units)
                .SingleOrDefaultAsync(x => x.Id == BookId);
        }

        public async Task<bool> UpdateBookAsync(Book bookForUpdate)
        {
            DataContext.Books.Update(bookForUpdate);
            var updated = await DataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Book>> GetBooksAsync()
        {
            var books = await DataContext.Books.ToListAsync();
            DataContext.Authors.Load();
            DataContext.Categories.Load();
            DataContext.Languages.Load();
            DataContext.BookAuthor.Load();
            DataContext.BookCategory.Load();
            return books;
            //.Include(x => x.BookAuthors)
            //.ThenInclude( bookauthors => bookauthors.Author)
            //.Include(x => x.BookCategories)
            //.ThenInclude(bookcategory => bookcategory.Category)
            //.Include(x => x.Units)
        }
    }
}
