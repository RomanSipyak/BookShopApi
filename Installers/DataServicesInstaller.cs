using BookShopApi.Data;
using BookShopApi.Services;
using BookShopApi.Services.ServicesImplementations;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Installers
{
    public class DataServicesInstaller : Iinstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //add configure for dbcontext and db connection
            services.AddDbContext<DataContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            //end configure for dbcontext and db connection

           
            services.AddScoped<IAuthorService, AuthorService>();
            services.AddScoped<IBookService, BookService>();
            services.AddScoped<ICategoryService, CategoryService>();
            services.AddScoped<ILanguageService, LanguageService>();
        }
    }
}
