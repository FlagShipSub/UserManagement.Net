using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;
using UserManagment.Models;

namespace UserManagment.Data
{
    public class ApplicationDbContext : IdentityDbContext<ApplicationUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options) { }

        public DbSet<ApplicationUser> ApplicationUsers { get; set; } 

        public DbSet<OtpValidation> OtpValidations { get; set; }
        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<OtpValidation>()
          .HasOne(o => o.ApplicationUser)
          .WithOne(u => u.OtpValidation)
          .HasForeignKey<OtpValidation>(o => o.Id);
        }
    }
}
