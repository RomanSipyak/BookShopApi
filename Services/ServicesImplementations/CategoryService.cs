using BookShopApi.Data;
using BookShopApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services.ServicesImplementations
{
    public class CategoryService : ICategoryService
    {
        private readonly DataContext Datacontext;

        public CategoryService(DataContext dataContext)
        {
            Datacontext = dataContext;
        }
        public async Task<bool> CreateCategoryAsync(Category category)
        {
            var categocyExist = await Datacontext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == category.Id || x.Title == category.Title);
            if (categocyExist != null)
            {
                return true;
            }
            await Datacontext.Categories.AddAsync(category);
            var created = await Datacontext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int categotyId)
        {
            var category = await GetCategoryByIdAsync(categotyId);
            if (category == null)
            {
                return false;
            }
            Datacontext.Categories.Remove(category);
            var deleted = await Datacontext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await Datacontext.Categories.Include(x => x.BookCategories).SingleOrDefaultAsync(x => x.Id == categoryId);
        }

        public async Task<Category> GetCategoryByTitleAsync(string title)
        {
            return await Datacontext.Categories.Include(x => x.BookCategories).SingleOrDefaultAsync(x => x.Title == title);
        }

        public async Task<bool> UpdateAuthorAsync(Category categotyForUpdate)
        {
            var existCategories = await Datacontext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Title == categotyForUpdate.Title);
            if (existCategories != null)
            {
                return false;
            }
            Datacontext.Categories.Update(categotyForUpdate);
            var updated = await Datacontext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await Datacontext.Categories.Include(x => x.BookCategories).ThenInclude(x => x.Book ).ToListAsync();
        }
    }
}
