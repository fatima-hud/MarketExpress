using MarketExpress.Core.Entity;
using MarketExpress.Core.Entity.Identity;
using MarketExpress.Infrastructure.Configurations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Emit;

namespace MarketExpress.Database
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.HasDefaultSchema("dbo");
            builder.ApplyConfigurationsFromAssembly(typeof(UserConfiguration).Assembly);
         
           



         
            builder.Entity<IdentityUser>().ToTable("Users", "identity");
            builder.Entity<IdentityRole>().ToTable("Roles", "identity");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles", "identity");
            builder.Entity<IdentityUserClaim<string>>().ToTable("UserClaims", "identity");
            builder.Entity<IdentityUserLogin<string>>().ToTable("UserLogins", "identity");
            builder.Entity<IdentityRoleClaim<string>>().ToTable("RoleClaims", "identity");
            builder.Entity<IdentityUserToken<string>>().ToTable("UserTokens", "identity");
            
            builder.Entity<User>()
                .HasMany(u => u.Messages)
                .WithOne(m => m.Sender)
                .HasForeignKey(m => m.SenderId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<User>()
                .HasMany(u => u.ComplaintsAsCustomer)
                .WithOne(c => c.Customer)
                .HasForeignKey(c => c.CustomerId)
                .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<User>()
              .HasMany(u => u.ComplaintsAsStoreOwner)
              .WithOne(c => c.StoreOwner)
              .HasForeignKey(c => c.StoreOwnerId)
              .OnDelete(DeleteBehavior.Restrict);
            builder.Entity<Complaint>().HasMany(c => c.Messages)
                .WithOne(m => m.Complaint)
                .HasForeignKey(m => m.ComplaintId)
                .OnDelete(DeleteBehavior.Cascade);


        }



       public DbSet<User> Users { get; set; }
        public DbSet<OtpCode> OtpCodes { get; set; }
        public DbSet<Category>Categories { get; set; }
        public DbSet<Store>Stores { get; set; }
       
     
        public DbSet<Rating> Ratings { get; set; }
        public DbSet<Cart>Carts { get; set; }   
        public DbSet<CartItem> CartItems { get; set; }  
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderItem> Orderitems { get; set; }
       
        public DbSet<ClothingItem> Clothingitems { get; set; }  
     // public DbSet<UserStoreRole> UserStoreRoles { get; set; }
        public DbSet<UserRefreshToken>UserRefreshTokens { get; set; }
       
               public DbSet<Product>Products { get; set; }
        public DbSet<UserDocument> Documents {  get; set; }
        public DbSet<Complaint> Complaints { get; set; }
        public DbSet<Message> Messages { get; set; }

    }
}
