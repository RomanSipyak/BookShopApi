using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BookShopApi.Installers
{
    public class SwaggerServicesInstaller : Iinstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {
            //add for work with json
            services.AddMvc(option => option.EnableEndpointRouting = false)
                .SetCompatibilityVersion(CompatibilityVersion.Version_3_0)
                .AddNewtonsoftJson(opt => opt.SerializerSettings.ReferenceLoopHandling = ReferenceLoopHandling.Ignore)
                .AddNewtonsoftJson(opt =>
                {
                    var resolver = opt.SerializerSettings.ContractResolver;
                    if (resolver != null)
                    {
                        var res = resolver as DefaultContractResolver;
                        res.NamingStrategy = null;  // <<!-- this removes the camelcasing

                    }
                });
            //add for work with json

            services.AddControllers();
            services.AddSwaggerGen(x =>
            {
                //new Info { Title = "Tweetbook API", Version = "v1" } in previous versions
                x.SwaggerDoc("v1", new Microsoft.OpenApi.Models.OpenApiInfo { Title = "Tweetbook API", Version = "v1" });
                var security = new Dictionary<string, IEnumerable<string>>
                {
                    { "Bearer", new string[0] }, //intialize type of indication that swagger need to support
                };

                x.AddSecurityDefinition(
                    "Bearer",
                    new OpenApiSecurityScheme
                       {
                           Description = "JWT Authorization header using the Bearer scheme. \r\n\r\n Enter 'Bearer' [space] and then your token in the text input below.\r\n\r\nExample: \"Bearer 12345abcdef\"",
                           Name = "Authorization",
                           In = ParameterLocation.Header,
                           Type = SecuritySchemeType.ApiKey,
                           Scheme = "Bearer",
                       });
                x.AddSecurityRequirement(new OpenApiSecurityRequirement()
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                            //Scheme = "oauth2",
                            //Name = "Bearer",
                            //In = ParameterLocation.Header,

                        },
                        new List<string>()
                    },
                });
            });
        }
    }
}
