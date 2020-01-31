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
        private readonly ILanguageService LanguageService;

        public LanguageController(ILanguageService languageService)
        {
            LanguageService = languageService;
        }

        [HttpPost(ApiRoutes.Languages.Create)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> CreateAsync([FromBody] CreateLaguageRequest languageRequest)//намагаємося замапити до категорії
        {
            var language = new Language
            {
                Title = languageRequest.Title
            };

            await LanguageService.CreateLanguageAsync(language);
            //вертає протокол запиту Вертає локал хост
            var baseUrl = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host.ToUriComponent()}";
            var locationUrl = baseUrl + "/" + ApiRoutes.Languages.Get.Replace("{languageTitle}", language.Title);
            return Created(locationUrl, language);
        }

        [HttpGet(ApiRoutes.Languages.GetAll)]
        public async Task<IActionResult> GetAllLanguagesAsync()
        {
            return Ok(await LanguageService.GetLanguagesAsync());
        }

        [HttpGet(ApiRoutes.Languages.Get)]
        public async Task<IActionResult> GetLangueageByTitle([FromRoute] string languageTitle)
        {
            var language = await LanguageService.GetLanguageByTitleAsync(languageTitle);
            if (language == null)
            {
                return NotFound();
            }

            return Ok(language);
        }

        [HttpPut(ApiRoutes.Languages.Update)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateAsync([FromRoute]int languageId, [FromBody] UpdateLanguageRequest request)
        {
            var language = await LanguageService.GetLanguageByIdAsync(languageId);
            language.Title = request.Title;
            var updated = await LanguageService.UpdateLanguageAsync(language);
            if (!updated)
            {
                return NotFound();
            }

            return Ok(language);
        }

        [HttpDelete(ApiRoutes.Languages.Delete)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteAsync([FromRoute]int languageId)
        {
            var languageDeleted = await LanguageService.DeleteLanguageByIdAsync(languageId);
            if (!languageDeleted)
            {
                return NotFound();
            }
            return Ok(languageDeleted);
        }
    }
}
