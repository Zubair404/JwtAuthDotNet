using System;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using JwtAuth.Entities;

namespace JwtAuth.Data
{
    public class UserDbContext : DbContext
    {
        public UserDbContext(DbContextOptions<UserDbContext> options) : base(options)
        {
        }

        public DbSet<User> Users { get; set; } = null!;

        // Suppress the PendingModelChangesWarning so `dotnet ef database update` won't throw.
        // NOTE: this is a workaround — you should still add a migration that captures the model changes.
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.ConfigureWarnings(w => w.Ignore(RelationalEventId.PendingModelChangesWarning));
            base.OnConfiguring(optionsBuilder);
        }
    }
}
