using BookShopApi.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services
{
    public interface ILanguageService
    {
        Task<bool> CreateLanguageAsync(Language language);
        Task<bool> DeleteLanguageByIdAsync(int languageId);
        Task<Language> GetLanguageByIdAsync(int languageId);
        Task<Language> GetLanguageByTitleAsync(string languageTitle);
        Task<bool> UpdateLanguageAsync(Language LanguageForUpdate);
        Task<List<Language>> GetLanguages();
    }
}
