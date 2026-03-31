using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using RestaurantSystem.Models;

namespace RestaurantSystem.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) { }

        public DbSet<MenuItem> MenuItems { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderDetail> OrderDetails { get; set; }
        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<IdentityUser>(entity => {
                entity.Property(e => e.Id).HasMaxLength(80);
                entity.Property(e => e.NormalizedUserName).HasMaxLength(128);
                entity.Property(e => e.NormalizedEmail).HasMaxLength(128);
            });
            builder.Entity<IdentityRole>(entity => {
                entity.Property(e => e.Id).HasMaxLength(80);
                entity.Property(e => e.NormalizedName).HasMaxLength(80);
            });
            builder.Entity<IdentityUserLogin<string>>(entity => {
                entity.Property(e => e.UserId).HasMaxLength(80);
                entity.Property(e => e.LoginProvider).HasMaxLength(80);
                entity.Property(e => e.ProviderKey).HasMaxLength(80);
            });
            builder.Entity<IdentityUserRole<string>>(entity => {
                entity.Property(e => e.UserId).HasMaxLength(80);
                entity.Property(e => e.RoleId).HasMaxLength(80);
            });
            builder.Entity<IdentityUserToken<string>>(entity => {
                entity.Property(e => e.UserId).HasMaxLength(80);
                entity.Property(e => e.LoginProvider).HasMaxLength(80);
                entity.Property(e => e.Name).HasMaxLength(80);
            });
        }
    }
}
