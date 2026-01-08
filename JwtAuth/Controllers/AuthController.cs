using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using JwtAuth.Entities;
using JwtAuth.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace JwtAuth.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController(IConfiguration configuration) : ControllerBase
    {

        public static User user = new User();
        [HttpPost]
        [Route("register")]
        public ActionResult<User> Register(UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var hashedPassword = new PasswordHasher<User>().HashPassword(user, request.password);
            
            user.username = request.username;
            user.password = hashedPassword;
            // Registration logic here

            return Ok();
        }
        [HttpPost("login")]
        public ActionResult<string> Login(UserDto request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            if(user.username != request.username)
            {
                return BadRequest("User not found.");
            }
            if(new PasswordHasher<User>().VerifyHashedPassword(user, user.password, request.password) == PasswordVerificationResult.Failed)
            {
                return BadRequest("Wrong password.");
            }
            string token = CreateToken(user);
            return Ok(token);
        }
        public string CreateToken(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.username)
            };
            var key = new SymmetricSecurityKey(System.Text.Encoding.UTF8.GetBytes( configuration.GetValue<string>("AppSettings:Token")! ));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha512Signature);
            // Token creation logic here
            var tokendescriptor = new JwtSecurityToken
            (
                issuer : configuration.GetValue<string>("AppSettings:Issuer"),
                audience : configuration.GetValue<string>("AppSettings:Audience"),
                claims : claims,
                expires : DateTime.UtcNow.AddDays(1),
                signingCredentials : creds
            );
            return new JwtSecurityTokenHandler().WriteToken(tokendescriptor);
        }
    }
}