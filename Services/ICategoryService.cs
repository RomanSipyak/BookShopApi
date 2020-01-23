﻿using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public interface ICategoryService
    {
        Task<bool> CreateCategoryAsync(Category category);
        Task<bool> DeleteCategoryByIdAsync(int categotyId);
        Task<Category> GetCategoryByIdAsync(int categoryId);
        Task<Category> GetCategoryByTitleAsync(string Title);
        Task<bool> UpdateAuthorAsync(Category categotyForUpdate);
        Task<List<Category>> GetCategoriesAsync();
    }
}
