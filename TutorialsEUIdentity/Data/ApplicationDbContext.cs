using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using TutorialsEUIdentity.Models;

namespace TutorialsEUIdentity.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {

        }
        //
        public DbSet<ProductModel> Product {  get; set; }
        public DbSet<CartItemModel> CartItems { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            builder.Entity<CartItemModel>()
                .HasOne(c => c.Product)
                .WithMany()
                .HasForeignKey(c => c.ProductID)
                .HasPrincipalKey(p => p.Id);
        }
    }
}
