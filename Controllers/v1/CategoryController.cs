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
    }
}
