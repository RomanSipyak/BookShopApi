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
        private readonly DataContext dataContext;

        public LanguageService(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        public async Task<bool> CreateLanguageAsync(Language language)
        {
            var existLanguage = await this.dataContext.Languages.AsNoTracking().SingleOrDefaultAsync(x => x.Id == language.Id || x.Title == language.Title);
            if (existLanguage != null)
            {
                return true;
            }

            await this.dataContext.Languages.AddAsync(language);
            var created = await this.dataContext.SaveChangesAsync();
            return created > 0;
        }

        public async Task<bool> DeleteLanguageByIdAsync(int languageId)
        {
            var language = await this.GetLanguageByIdAsync(languageId);
            if (language == null)
            {
                return false;
            }

            this.dataContext.Languages.Remove(language);

            var deleted = await this.dataContext.SaveChangesAsync();

            return deleted > 0;
        }

        public async Task<Language> GetLanguageByIdAsync(int languageId)
        {
            return await this.dataContext.Languages.Include(x => x.Books).SingleOrDefaultAsync(x => x.Id == languageId);
        }

        public async Task<Language> GetLanguageByTitleAsync(string languageTitle)
        {
            return await this.dataContext.Languages.Include(x => x.Books).SingleOrDefaultAsync(x => x.Title == languageTitle);
        }

        public async Task<bool> UpdateLanguageAsync(Language languageForUpdate)
        {
            var existLanguage = await this.dataContext.Languages.AsNoTracking().SingleOrDefaultAsync(x => x.Title == languageForUpdate.Title);
            if (existLanguage != null)
            {
                return false;
            }

            this.dataContext.Languages.Update(languageForUpdate);
            var updated = await this.dataContext.SaveChangesAsync();
            return updated > 0;
        }

        public async Task<List<Language>> GetLanguagesAsync()
        {
            return await this.dataContext.Languages.Include(x => x.Books).ToListAsync();
        }
    }
}
