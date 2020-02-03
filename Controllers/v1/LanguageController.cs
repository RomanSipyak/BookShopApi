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
    public class LanguageController : Controller
    {
        private readonly ILanguageService languageService;

        public LanguageController(ILanguageService languageService)
        {
            this.languageService = languageService;
        }

        [HttpPost(ApiRoutes.Languages.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLaguageRequest languageRequest)//намагаємося замапити до категорії
        {
            var language = new Language
            {
                Title = languageRequest.Title,
            };

            await this.languageService.CreateLanguageAsync(language);
            //вертає протокол запиту Вертає локал хост
            var baseUrl = $"{this.HttpContext.Request.Scheme}://{this.HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Languages.Get.Replace("{languageTitle}", language.Title);
            return this.Created(locationUrl, language);
        }

        [HttpGet(ApiRoutes.Languages.GetAll)]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            return this.Ok(await this.languageService.GetLanguagesAsync());
        }

        [HttpGet(ApiRoutes.Languages.Get)]
        public async Task<IActionResult> GetLangueageByTitle([FromRoute] string languageTitle)
        {
            var language = await this.languageService.GetLanguageByTitleAsync(languageTitle);
            if (language == null)
            {
                return this.NotFound();
            }

            return this.Ok(language);
        }

        [HttpPut(ApiRoutes.Languages.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int languageId, [FromBody] UpdateLanguageRequest request)
        {
            var language = await this.languageService.GetLanguageByIdAsync(languageId);
            language.Title = request.Title;
            var updated = await this.languageService.UpdateLanguageAsync(language);
            if (!updated)
            {
                return this.NotFound();
            }

            return this.Ok(language);
        }

        [HttpDelete(ApiRoutes.Languages.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int languageId)
        {
            var languageDeleted = await this.languageService.DeleteLanguageByIdAsync(languageId);
            if (!languageDeleted)
            {
                return this.NotFound();
            }

            return this.Ok(languageDeleted);
        }
    }
}
