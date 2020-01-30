using BookShopApi.Data;
using BookShopApi.Options;
using BookShopApi.Services;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BookShopApi.Installers
{
    public class IdentityServicesInstaller : Iinstaller
    {
        public void InstallServices(IServiceCollection services, IConfiguration configuration)
        {   //start configure JWT
            //we can get our  "JwtSettings": { "SecretKey": "some key" }
            //like configuration["JwtSettings:SecretKey"].ToString();
            var jwtSettings = new JwtSettings();
            configuration.Bind(nameof(JwtSettings), jwtSettings);//that part code binding our jwt setting object that have one property (secretKey) with our settings.json that have the same jsonObject with jwt secutity key(mapping them) 
            services.AddSingleton(jwtSettings);
            // configure jwt authentication
            services.AddScoped<IIdentityService, IdentityService>();    

            var key = Encoding.ASCII.GetBytes(jwtSettings.SecretKey);
            services.AddAuthentication(x =>
            {
                x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
                x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            }).AddJwtBearer(x => //setting for how we work with token
            {
                x.RequireHttpsMetadata = false;
                x.SaveToken = true; //don`t save token in db
                x.TokenValidationParameters = new TokenValidationParameters //It is how we need validate our token that we take from our client
                {
                    ValidateIssuerSigningKey = true, //for validating our token with secret key
                    IssuerSigningKey = new SymmetricSecurityKey(key),// that provide encription of signature part by sekret key
                    ValidateIssuer = false,
                    ValidateAudience = false,//it is like who generate this token and we compare it when we get that(read in documentation)
                    RequireExpirationTime = false,
                    ValidateLifetime = true
                };
            });
            // configure jwt authentication
            //end configure JWT
            //add configure for identity and db connection
            //services.AddIdentity<User, IdentityRole>().AddEntityFrameworkStores<DataContext>();
            services.AddDefaultIdentity<IdentityUser>(options => options.SignIn.RequireConfirmedAccount = true)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<DataContext>();
        }
    }
}
