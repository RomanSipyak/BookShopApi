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
        private readonly DataContext dataContext;

        public AuthorService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> CreateAuthorAsync(Author author)
        {
            var existingAuthor = await this.dataContext.Authors.AsNoTracking().SingleOrDefaultAsync(x => x.Id == author.Id);
            if (existingAuthor != null)
            {
                return true;
            }

            await this.dataContext.Authors.AddAsync(author);
            var created = await this.dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteAuthorByIdAsync(int authorId)
        {
            var author = await this.GetAuthorByIdAsync(authorId);
            if (author == null)
            {
                return false;
            }

            this.dataContext.Authors.Remove(author);
            var deleted = await this.dataContext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Author> GetAuthorByIdAsync(int authorId)
        {
            return await this.dataContext.Authors
                .Include(x => x.BookAuthors)
                .SingleOrDefaultAsync(x => x.Id == authorId);
        }

        public Author GetAuthorById(int authorId)
        {
            return this.dataContext.Authors
                .Include(x => x.BookAuthors)
                .SingleOrDefault(x => x.Id == authorId);
        }

        public async Task<bool> UpdateAuthorAsync(Author authorForUpdate)
        {
            this.dataContext.Authors.Update(authorForUpdate);
            var updated = await this.dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Author>> GetAuthorsAsync()
        {
            return await this.dataContext.Authors.Include(x => x.BookAuthors).ThenInclude(xa => xa.Book).ToListAsync();
        }
    }
}
