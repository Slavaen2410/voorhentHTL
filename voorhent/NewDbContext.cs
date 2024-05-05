using Microsoft.EntityFrameworkCore;

namespace voorhent.Models
{
    public class NewDbContext : DbContext
    {
        public NewDbContext(DbContextOptions<NewDbContext> options) : base(options) { }

        public DbSet<Hotel> Hotels { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
 
        }
    }
}