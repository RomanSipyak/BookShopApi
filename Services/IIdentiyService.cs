using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Contracts.v1.Responses;
using BookShopApi.Domain;
using BookShopApi.Domain.Models;

namespace BookShopApi.Services
{
    public interface IIdentityService
    {
        Task<AuthentificanionResult> RegisterAsync(string email, string password);

        Task<AuthentificanionResult> LoginAsync(string email, string password);
        
        Task<object> GetUserProfileAsync(string userId);

        Task<List<UserResponse>> GetAllUsersAsync();

        Task<bool> UpdateUserAsync(UpdateUserRequest updateUserRequest);

        Task<bool> DeleteUserByEmail(string email);

        Task<object> GetAllRolesAsync();
    }
}
