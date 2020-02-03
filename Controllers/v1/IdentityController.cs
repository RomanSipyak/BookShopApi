using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using System.Threading.Tasks;
using Tweetbook.Contracts;
using BookShopApi.Contracts.v1.Requests;
using BookShopApi.Contracts.v1.Responses;
using BookShopApi.Services;

namespace BookShopApi.Controllers.v1
{
    public class IdentityController : Controller
    {
        private readonly IIdentityService identityService;

        public IdentityController(IIdentityService identityService)
        {
            this.identityService = identityService;
        }

        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!this.ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = this.ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage)),
                });
            }

            var authResponse = await this.identityService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return this.BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }

            return this.Ok(new AuthSuccessresponse
            {
                Token = authResponse.Token,
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await this.identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return this.BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors,
                });
            }

            return this.Ok(new AuthSuccessresponse
            {
                Token = authResponse.Token,
            });
        }

        [HttpGet(ApiRoutes.Identity.GetProfile)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Object> GetUserProfileAsync()
        {
            string userId = this.User.Claims.First(c => c.Type == "id").Value;
            var userDetails = await this.identityService.GetUserProfileAsync(userId);
            return this.Ok(userDetails);
        }

        [HttpGet(ApiRoutes.Identity.GetAllUsers)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {
            var userResponses = await this.identityService.GetAllUsersAsync();
            return this.Ok(userResponses);
        }

        [HttpGet(ApiRoutes.Identity.GetAllRoles)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            return this.Ok(await this.identityService.GetAllRolesAsync());
        }

        [HttpDelete(ApiRoutes.Identity.DeleteUserByEmail)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUserByEmailAsync([FromRoute] string userEmail)
        {
            var userDeleted = await this.identityService.DeleteUserByEmail(userEmail);
            if (!userDeleted)
            {
                return this.NotFound();
            }

            return this.Ok(userDeleted);
        }

        [HttpPut(ApiRoutes.Identity.UpdateUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UpdateUserRequest user)
        {
            var userUpdated = await identityService.UpdateUserAsync(user);
            if (!userUpdated)
            {
                return this.NotFound();
            }

            return this.Ok(userUpdated);
        }
    }
}