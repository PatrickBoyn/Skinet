using Core.Entities;
using Microsoft.EntityFrameworkCore;

namespace Core.Data
{

    public class StoreContext : DbContext
    {
        public StoreContext(DbContextOptions<StoreContext> options) : base(options) { }
        
        public DbSet<Products> Products { get; set; }
    }

}