using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Domain.Models;
using BookShopApi.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Tweetbook.Contracts;

namespace BookShopApi.Controllers.v1
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            this.categoryService = categoryService;
        }

        [HttpPost(ApiRoutes.Categories.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest categoryRequest)//намагаємося замапити до категорії
        {
            var category = new Category
            {
                Title = categoryRequest.Title,
            };

            await categoryService.CreateCategoryAsync(category);
            //вертає протокол запиту Вертає локал хост
            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Categories.Get.Replace("{categoryTitle}", category.Title);
            return this.Created(locationUrl, category);
        }

        [HttpGet(ApiRoutes.Categories.GetAll)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return this.Ok(await this.categoryService.GetCategoriesAsync());
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        public async Task<IActionResult> GetCategotyByTitle([FromRoute] string categoryTitle)
        {
            var category = await this.categoryService.GetCategoryByTitleAsync(categoryTitle);
            if (category == null)
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

        [HttpPut(ApiRoutes.Categories.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var category = await this.categoryService.GetCategoryByIdAsync(categoryId);
            category.Title = request.Title;
            var updated = await this.categoryService.UpdateCategoryAsync(category);
            if (!updated)
            {
                return this.NotFound();
            }

            return this.Ok(category);
        }

        [HttpDelete(ApiRoutes.Categories.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int CategoryId)
        {
            var categoryDeleted = await this.categoryService.DeleteCategoryByIdAsync(CategoryId);
            if (!categoryDeleted)
            {
                return this.NotFound();
            }

            return this.Ok(categoryDeleted);
        }
    }
}
