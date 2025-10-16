using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sudoku.ApiService.Models.DbModels;
using System.Security.Cryptography;

namespace Sudoku.ApiService.Data
{
    public class AppDbContext : IdentityDbContext<UserModel, RoleModel, string>
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        // Enities


        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            var roles = new List<RoleModel>
            {
                new RoleModel { Id = Guid.NewGuid().ToString(), Name = "Admin", NormalizedName = "ADMIN" },
                new RoleModel { Id = Guid.NewGuid().ToString(), Name = "User", NormalizedName = "USER" }
            };

            builder.Entity<RoleModel>().HasData(roles);

            // Relationships
        }
    }
}
