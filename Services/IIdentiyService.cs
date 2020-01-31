using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApi.Domain;
using BookShopApi.Domain.Models;

namespace BookShopApi.Services
{
    public interface IIdentityService
    {
        Task<AuthentificanionResult> RegisterAsync(string email, string password);

        Task<AuthentificanionResult> LoginAsync(string email, string password);
        
        Task<Object> GetUserProfileAsync(string userId);

        Task<object> GetAllUsers();
    }
}
