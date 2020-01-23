using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public interface IAuthorService
    {
        Task<bool> CreateAuthorAsync(Author author);
        Task<bool> DeleteAuthorByIdAsync(int authorId);
        Task<Author> GetAuthorByIdAsync(int authorId);
        Task<bool> UpdateAuthorAsync(Author authorForUpdate);
        Task<List<Author>> GetAuthorsAsync();
        //Task<Book> GetBookByIdAsync(int BookId);
        //Task<List<Book>> GetBooksAsync();
    }
}
