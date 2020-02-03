using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using BookShopApi.Domain;
using BookShopApi.Options;
using BookShopApi.Domain.Models;
using Microsoft.EntityFrameworkCore;
using BookShopApi.Contracts.v1.Responses;
using BookShopApi.Contracts.v1.Requests;

namespace BookShopApi.Services
{

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtSettings jwtSettings;
        private readonly RoleManager<IdentityRole> roleManager;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.jwtSettings = jwtSettings;
            this.roleManager = roleManager;
        }

        public async Task<object> GetUserProfileAsync(string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var userRoles = await this.userManager.GetRolesAsync(user);
            return new
            {
                user.Email,
                userRoles,
            };
        }

        public async Task<bool> DeleteUserByEmail(string email)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return false;
            }

            var deleted = await this.userManager.DeleteAsync(user);
            if (deleted.Succeeded)
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        // find bug with async
        public async Task<List<UserResponse>> GetAllUsersAsync()
        {
            var users = this.userManager.Users.ToList();
            var usersResponses = new List<UserResponse>();

            foreach (var user in users)
            {
                IList<string> userRoles = await this.userManager.GetRolesAsync(user);
                usersResponses.Add(new UserResponse
                {
                    Email = user.Email,
                    userRoles = userRoles.Select(x => new Role { Title = x}).ToList(),
                });
            }

            return usersResponses;
        }

        public async Task<AuthentificanionResult> LoginAsync(string email, string password)
        {
            var user = await this.userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User does not exists",
                    },
                };
            }

            var userHasValidPassword = await this.userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User/password combination is wrong ",
                    },
                };
            }

            return await GenerateAuthentificationResultForUser(user);
        }

        public async Task<AuthentificanionResult> RegisterAsync(string email, string password)
        {
            var existingUser = await this.userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User with this email address already exists",
                    },
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email,
            };
            var createdUser = await this.userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthentificanionResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description),
                };
            }

            return await GenerateAuthentificationResultForUser(newUser);
        }

        private async Task<AuthentificanionResult> GenerateAuthentificationResultForUser(IdentityUser newUser)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, newUser.Email),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Email, newUser.Email),
                new Claim("id", newUser.Id),
            };
            //insert spec roles claims to our jwt
            var userRoles = await this.userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await this.roleManager.FindByNameAsync(userRole);
                if (role == null)
                {
                    continue;
                }

                var roleClaims = await this.roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                    {
                        continue;
                    }

                    claims.Add(roleClaim);
                }
            }

            //insert spec roles claims to our jwt
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(this.jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature),
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthentificanionResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token),
            };
        }

        public async Task<object> GetAllRolesAsync()
        {
            var roles = await roleManager.Roles.ToListAsync();
            return roles.Select(x => new RoleResponse { Title = x.Name }).ToList();
        }

        public async Task<bool> UpdateUserAsync(UpdateUserRequest updateUserRequest)
        {
                var existUsers = await this.userManager.FindByEmailAsync(updateUserRequest.Email);
                if (existUsers == null)
                {
                    return false;
                }

                // получем список ролей пользователя
                var userRoles = await this.userManager.GetRolesAsync(existUsers);
                // получаем список ролей, которые были добавлены
                var addedRoles = updateUserRequest.userRoles.Select(x => x.Title).Except(userRoles);
                // получаем роли, которые были удалены
                var removedRoles = userRoles.Except(updateUserRequest.userRoles.Select(x => x.Title));

                await this.userManager.AddToRolesAsync(existUsers, addedRoles);

                await this.userManager.RemoveFromRolesAsync(existUsers, removedRoles);

                return true;
        }
    }
}
