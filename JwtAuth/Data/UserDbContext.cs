using System;
using Microsoft.EntityFrameworkCore;
using JwtAuth.Entities;

namespace JwtAuth.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;
    }
}
