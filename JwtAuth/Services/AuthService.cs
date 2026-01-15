using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using JwtAuth.Data;
using JwtAuth.Entities;
using JwtAuth.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Services
{
    public class AuthService(UserDbContext context,IConfiguration configuration) : IAuthService
    {
        public async Task<User?> RegisterAsync(UserDto request)
        {
            if(await context.Users.AnyAsync(u => u.username == request.username))
            {
                return null;
            }
            
            var user = new User();
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.password);
            
            user.username = request.username;
            user.password = hashedPassword;

            context.Users.Add(user);
            await context.SaveChangesAsync();
            return user;
        }

        public async Task<string?> LoginAsync(UserDto request)
        {
            var user = await context.Users.FirstOrDefaultAsync(u => u.username == request.username);
            if (user == null)
            {
                return null;
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.password, request.password) == PasswordVerificationResult.Failed)
            {
                return null;
            }
            return CreateToken(user);
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username),
                new Claim(ClaimTypes.NameIdentifier, user.id.ToString())
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes(configuration.GetValue<string>("AppSettings:Token")!));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // Token creation logic here
            var tokendescriptor = new JwtSecurityToken
            (
                issuer: configuration.GetValue<string>("AppSettings:Issuer"),
                audience: configuration.GetValue<string>("AppSettings:Audience"),
                claims: claims,
                expires: DateTime.UtcNow.AddDays(1),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokendescriptor);
        }
    }
}
