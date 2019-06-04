using e_commerce.webapi.Models;
using Microsoft.EntityFrameworkCore;

namespace e_commerce.webapi.Infrastructure
{
    public class AppDbContext:DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options):base(options){

        }
        public DbSet<ClienteLogin> Users { get; set; }
        public DbSet<CustomerDetail> Me { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<SubCategory> SubCategories { get; set; }
        public DbSet<ProductImage> ProductImages { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<ProductListDTO> ProductsListDTO { get; set; }
        public DbSet<IdentityResult> IdentityResult { get; set; }
        public DbSet<OrderModel> Orders { get; set; }
        public DbSet<EnterpriseImage> EmpresaImagen { get; set; }
        public DbSet<OrderDetail> OrderDetail { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Status> Status { get; set; }
        public DbSet<AccountResume> AccountResumes { get; set; }

       
      
    }
}