using BookShopApi.Data;
using BookShopApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Services.ServicesImplementations
{
    public class LanguageService : ILanguageService
    {
        private readonly DataContext DataContext;

        public LanguageService(DataContext dataContext)
        {
            DataContext = dataContext;
        }

        public async Task<bool> CreateLanguageAsync(Language language)
        {
            var existLanguage = await DataContext.Languages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == language.Id || x.Title == language.Title);
            if (existLanguage != null)
            {
                return true;
            }
            await DataContext.Languages.AddAsync(language);
            var created = await DataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteLanguageByIdAsync(int languageId)
        {
            var language = await GetLanguageByIdAsync(languageId);
            if (language == null)
            {
                return false;
            }
            DataContext.Languages.Remove(language);

            var deleted = await DataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<Language> GetLanguageByIdAsync(int languageId)
        {
            return await DataContext.Languages.Include(x => x.Books).SingleOrDefaultAsync(x => x.Id == languageId);
        }

        public async Task<Language> GetLanguageByTitleAsync(string languageTitle)
        {
            return await DataContext.Languages.Include(x => x.Books).SingleOrDefaultAsync(x => x.Title == languageTitle);
        }

        public async Task<bool> UpdateLanguageAsync(Language languageForUpdate)
        {
            var existLanguage = await DataContext.Languages.AsNoTracking().SingleOrDefaultAsync(x => x.Title == languageForUpdate.Title);
            if (existLanguage != null)
            {
                return false;
            }
            DataContext.Languages.Update(languageForUpdate);
            var updated = await DataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Language>> GetLanguagesAsync()
        {
            return await DataContext.Languages.Include(x => x.Books).ToListAsync();
        }
    }
}
