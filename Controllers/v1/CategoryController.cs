using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Domain.Models;
using BookShopApi.Services;
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
        private readonly ICategoryService CategoryService;

        public CategoryController(ICategoryService categoryService)
        {
            CategoryService = categoryService;
        }

        [HttpPost(ApiRoutes.Categories.Create)]
        public async Task<IActionResult> CreateAsync([FromBody] CreateCategoryRequest categoryRequest)//намагаємося замапити до категорії
        {
            var category = new Category
            {
                Title = categoryRequest.Title
            };

            await CategoryService.CreateCategoryAsync(category);
            //вертає протокол запиту Вертає локал хост
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Categories.Get.Replace("{categoryTitle}", category.Title);
            return Created(locationUrl, category);
        }

        [HttpGet(ApiRoutes.Categories.GetAll)]
        public async Task<IActionResult> GetAllCategoriesAsync()
        {
            return Ok(await CategoryService.GetCategoriesAsync());
        }

        [HttpGet(ApiRoutes.Categories.Get)]
        public async Task<IActionResult> GetCategotyByTitle([FromRoute] string categoryTitle)
        {
            var category = await CategoryService.GetCategoryByTitleAsync(categoryTitle);
            if(category == null)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpPut(ApiRoutes.Categories.Update)]
        public async Task<IActionResult> UpdateAsync([FromRoute]int categoryId, [FromBody] UpdateCategoryRequest request)
        {
            var category = await CategoryService.GetCategoryByIdAsync(categoryId);
            category.Title = request.Title;
            var updated = await CategoryService.UpdateCategoryAsync(category);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(category);
        }

        [HttpDelete(ApiRoutes.Categories.Delete)]
        public async Task<IActionResult> DeleteAsync([FromRoute]int categoryId)
        {
            var categoryDeleted = await CategoryService.DeleteCategoryByIdAsync(categoryId);
            if (!categoryDeleted)
            {
                return NotFound();
            }
            return Ok(categoryDeleted);
        }
    }
}
