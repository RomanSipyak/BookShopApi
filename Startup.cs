using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApi.Data;
using BookShopApi.Domain;
using BookShopApi.Installers;
using BookShopApi.Options;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.HttpsPolicy;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.Swagger;
using Swashbuckle.Swagger;

namespace BookShopApi
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            services.InstallAllServices(Configuration);
            //services.AddControllersWithViews();
            //services.AddRazorPages();
            services.AddCors();
           
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {   
                  
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            //app.UseMvc();
            //for computing statics files
            app.UseDefaultFiles();
            app.UseStaticFiles();
            //for computing statics files
            //app.UseSpaStaticFiles();
            //app.UseSpa(spa =>
            //{
            //    // To learn more about options for serving an Angular SPA from ASP.NET Core,
            //    // see https://go.microsoft.com/fwlink/?linkid=864501

            //    spa.Options.SourcePath = "ClientApp";

            //    if (env.IsDevelopment())
            //    {
            //        spa.UseProxyToSpaDevelopmentServer("http://localhost:4200");
            //        //spa.UseAngularCliServer(npmScript: "start");
            //    }
            //});
            //take api access to take requests from 4200
            app.UseCors(builder => builder.WithOrigins("http://localhost:4200")
            .AllowAnyHeader()
            .AllowAnyMethod());

            //swagger configure
            var swaggerOptions = new Options.SwaggerOptions();
            Configuration.GetSection(nameof(Options.SwaggerOptions)).Bind(swaggerOptions);
            app.UseSwagger(option => { option.RouteTemplate = swaggerOptions.JsonRoute; });
            app.UseSwaggerUI(options => { options.SwaggerEndpoint(swaggerOptions.UIEndPoint, swaggerOptions.Description); });
            //end of swagger configure

            app.UseHttpsRedirection();

            app.UseRouting();

            //Add authorization and authentication
            app.UseAuthentication();
            app.UseAuthorization();
            //Add authorization and authentication
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}
