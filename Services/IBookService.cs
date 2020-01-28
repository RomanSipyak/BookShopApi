using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public interface IBookService
    {
        Task<bool> CreateBookAsync(Book book);
        Task<bool> DeleteBookByIdAsync(int BookId);
        Task<Book> GetBookByIdAsync(int BookId);
        Task<bool> UpdateBookAsync(Book bookForUpdate);
        Task<List<Book>> GetBooksAsync();
    }
}
