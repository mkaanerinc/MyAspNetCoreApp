using Microsoft.EntityFrameworkCore;

namespace MyAspNetCoreApp.Web.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> contextOptions) : base(contextOptions)
        {
            
        }

        public DbSet<Product> Products { get; set; }
    }
}
