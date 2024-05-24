using Microsoft.EntityFrameworkCore;
using WatchWeb.Model.Entities;

namespace WatchWeb.Model.EFContext
{
    public partial class DataContext : DbContext
    {
        public DbSet<Address> Address { get; set; }
        public DbSet<Category> Category { get; set; }
        public DbSet<Document> Document { get; set; }
        public DbSet<Order> Order { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Permission> Permission { get; set; }
        public DbSet<Product> Product { get; set; }
        public DbSet<Role> Role { get; set; }
        public DbSet<RolePermission> RolePermission { get; set; }
        public DbSet<UserAccount> UserAccount { get; set; }
        public DbSet<UserRole> UserRole { get; set; }
        public DbSet<Warranty> Warranty { get; set; }
        public DbSet<ProductCategory> ProductCategory { get; set; }


        public DataContext() { }

        public DataContext(DbContextOptions<DataContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<UserRole>().HasKey(x => new { x.RoleId, x.UserAccountId });
            modelBuilder.Entity<RolePermission>().HasKey(x => new { x.RoleId, x.PermissionId });
            modelBuilder.Entity<ProductCategory>().HasKey(x => new { x.ProductId, x.CategoryId });
            modelBuilder.Entity<UserAccount>().HasKey(x => x.Id);
        }
    }
}
