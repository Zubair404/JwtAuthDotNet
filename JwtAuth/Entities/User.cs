using System;

namespace JwtAuth.Entities
{
    public class User
    {
        public Guid id { get; set; } 
        public string username { get; set; } = string.Empty;
        public string password { get; set; } = string.Empty;
    }
}
