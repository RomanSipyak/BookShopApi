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
        private readonly DataContext datacontext;

        public CategoryService(DataContext dataContext)
        {
            this.datacontext = dataContext;
        }

        public async Task<bool> CreateCategoryAsync(Category category)
        {
            var categocyExist = await this.datacontext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Id == category.Id || x.Title == category.Title);
            if (categocyExist != null)
            {
                return true;
            }

            await this.datacontext.Categories.AddAsync(category);
            var created = await this.datacontext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteCategoryByIdAsync(int categotyId)
        {
            var category = await this.GetCategoryByIdAsync(categotyId);
            if (category == null)
            {
                return false;
            }

            this.datacontext.Categories.Remove(category);
            var deleted = await this.datacontext.SaveChangesAsync();
            return deleted > 0;
        }

        public async Task<Category> GetCategoryByIdAsync(int categoryId)
        {
            return await this.datacontext.Categories.Include(x => x.BookCategories).SingleOrDefaultAsync(x => x.Id == categoryId);
        }

        public Category GetCategoryById(int categoryId)
        {
            return this.datacontext.Categories.Include(x => x.BookCategories).SingleOrDefault(x => x.Id == categoryId);
        }

        public async Task<Category> GetCategoryByTitleAsync(string title)
        {
            return await this.datacontext.Categories.Include(x => x.BookCategories).SingleOrDefaultAsync(x => x.Title == title);
        }

        public async Task<bool> UpdateCategoryAsync(Category categotyForUpdate)
        {
            var existCategories = await this.datacontext.Categories.AsNoTracking().SingleOrDefaultAsync(x => x.Title == categotyForUpdate.Title);
            if (existCategories != null)
            {
                return false;
            }

            this.datacontext.Categories.Update(categotyForUpdate);
            var updated = await this.datacontext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Category>> GetCategoriesAsync()
        {
            return await this.datacontext.Categories.Include(x => x.BookCategories).ThenInclude(x => x.Book).ToListAsync();
        }
    }
}
