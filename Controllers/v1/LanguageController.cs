using BookShopApi.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Controllers.v1
{
    public class LanguageController : Controller
    {
        private readonly ILanguageService LanguageService;

        public LanguageController(ILanguageService languageService)
        {
            LanguageService = languageService;
        }
    }
}
