using System;
using Microsoft.Identity.Client;

namespace JwtAuth.Entities
{
    public class User
    {
        public Guid id { get; set; } 
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
        public string role { get; set; } = string.Empty;
        public string? refreshToken { get; set; } = string.Empty;
        public DateTime? refreshTokenExpiryTime { get; set; }
    }
}
