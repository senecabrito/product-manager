using Microsoft.EntityFrameworkCore;
using ProductManager.Data.Models;

namespace ProductManager.Data.Database.Context
{
    public class MSSQLContext : DbContext
    {
        public MSSQLContext(DbContextOptions<MSSQLContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
    }
}
