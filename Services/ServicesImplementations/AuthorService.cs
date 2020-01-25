using BookShopApi.Data;
using BookShopApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services.ServicesImplementations
{
    public class AuthorService : IAuthorService
    {
        private readonly DataContext DataContext;

        public AuthorService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<bool> CreateAuthorAsync(Author author)
        {
            var existingAuthor = await DataContext.Authors.AsNoTracking().SingleOrDefaultAsync(x => x.Id == author.Id);
            if (existingAuthor != null)
            {
                return true;
            }

            await DataContext.Authors.AddAsync(author);
            var created = await DataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteAuthorByIdAsync(int authorId)
        {
            var author = await GetAuthorByIdAsync(authorId);
            if (author == null)
            {
                return false;
            }

            DataContext.Authors.Remove(author);
            var deleted = await DataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await DataContext.Authors
                .Include(x => x.BookAuthors)
                .SingleOrDefaultAsync(x => x.Id == authorId);
        }

        public async Task<bool> UpdateAuthorAsync(Author authorForUpdate)
        {
            DataContext.Authors.Update(authorForUpdate);
            var updated = await DataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await DataContext.Authors.Include(x => x.BookAuthors).ThenInclude(xa => xa.Book).ToListAsync();
        }
    }
}
