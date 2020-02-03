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
        private readonly IIdentityService _identityService;
        public IdentityController(IIdentityService identityService)
        {
            _identityService = identityService;
        }
        [HttpPost(ApiRoutes.Identity.Register)]
        public async Task<IActionResult> Register([FromBody] UserRegistrationRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = ModelState.Values.SelectMany(x => x.Errors.Select(xx => xx.ErrorMessage))
                });
            }
            var authResponse = await _identityService.RegisterAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessresponse
            {
                Token = authResponse.Token
            });
        }

        [HttpPost(ApiRoutes.Identity.Login)]
        public async Task<IActionResult> Login([FromBody] UserLoginRequest request)
        {
            var authResponse = await _identityService.LoginAsync(request.Email, request.Password);
            if (!authResponse.Success)
            {
                return BadRequest(new AuthFailedResponse
                {
                    Errors = authResponse.Errors
                });
            }
            return Ok(new AuthSuccessresponse
            {
                Token = authResponse.Token
            });
        }

        
        [HttpGet(ApiRoutes.Identity.GetProfile)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public async Task<Object> GetUserProfileAsync()
        {
            string userId = User.Claims.First(c => c.Type == "id").Value;
           var userDetails =  await _identityService.GetUserProfileAsync(userId);
            return Ok(userDetails);
        }

        [HttpGet(ApiRoutes.Identity.GetAllUsers)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllUsersAsync()
        {   
            var userResponses = await _identityService.GetAllUsersAsync();
            return Ok(userResponses);
        }

        [HttpGet(ApiRoutes.Identity.GetAllRoles)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> GetAllRolesAsync()
        {
            return  Ok(await _identityService.GetAllRolesAsync());
        }

        [HttpDelete(ApiRoutes.Identity.DeleteUserByEmail)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> DeleteUserByEmailAsync([FromRoute] string userEmail)
        {
            var userDeleted = await _identityService.DeleteUserByEmail(userEmail);
            if (!userDeleted)
            {
                return NotFound();
            }
            return Ok(userDeleted);
        }

        [HttpPut(ApiRoutes.Identity.UpdateUser)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme, Roles = "Admin")]
        public async Task<IActionResult> UpdateUserRolesAsync([FromBody] UpdateUserRequest user)
        {
            var userUpdated = await _identityService.UpdateUserAsync(user);
            if (!userUpdated)
            {
                return NotFound();
            }
            return Ok(userUpdated);
        }
    }
}