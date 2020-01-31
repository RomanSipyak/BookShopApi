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

namespace BookShopApi.Services
{

    public class IdentityService : IIdentityService
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly JwtSettings _jwtSettings;
        private readonly RoleManager<IdentityRole> _roleManager;

        public IdentityService(UserManager<IdentityUser> userManager, JwtSettings jwtSettings, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _jwtSettings = jwtSettings;
            _roleManager = roleManager;
        }

        public async Task<object> GetUserProfileAsync(string userId)
        {
            var user = await _userManager.FindByIdAsync(userId);
            var userRoles = await _userManager.GetRolesAsync(user);
            return new
            {
                user.Email,
                userRoles
            };
        }

        public async Task<object> GetAllUsers()
        {
            var users = await _userManager.Users.ToListAsync();
            var responseUsers = new List<Object>();

            users.ForEach(async x => responseUsers.Add(new
            {
                x.Email,
                userRoles = await _userManager.GetRolesAsync(x)
            }
                ));
            return responseUsers;
        }



        public async Task<AuthentificanionResult> LoginAsync(string email, string password)
        {
            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User does not exists"
                    }
                };
            }
            var userHasValidPassword = await _userManager.CheckPasswordAsync(user, password);
            if (!userHasValidPassword)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User/password combination is wrong "
                    }
                };
            }
            return await GenerateAuthentificationResultForUser(user);
        }

        public async Task<AuthentificanionResult> RegisterAsync(string email, string password)
        {
            var existingUser = await _userManager.FindByEmailAsync(email);
            if (existingUser != null)
            {
                return new AuthentificanionResult
                {
                    Errors = new[]
                    {
                        "User with this email address already exists"
                    }
                };
            }

            var newUser = new IdentityUser
            {
                Email = email,
                UserName = email
            };
            var createdUser = await _userManager.CreateAsync(newUser, password);
            if (!createdUser.Succeeded)
            {
                return new AuthentificanionResult
                {
                    Errors = createdUser.Errors.Select(x => x.Description)
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
                new Claim("id", newUser.Id)
            };
            //insert spec roles claims to our jwt
            var userRoles = await _userManager.GetRolesAsync(newUser);
            foreach (var userRole in userRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole));
                var role = await _roleManager.FindByNameAsync(userRole);
                if (role == null) continue;
                var roleClaims = await _roleManager.GetClaimsAsync(role);

                foreach (var roleClaim in roleClaims)
                {
                    if (claims.Contains(roleClaim))
                        continue;

                    claims.Add(roleClaim);
                }
            }
            //insert spec roles claims to our jwt


            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = DateTime.UtcNow.AddHours(2),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return new AuthentificanionResult
            {
                Success = true,
                Token = tokenHandler.WriteToken(token)
            };
        }
    }
}
