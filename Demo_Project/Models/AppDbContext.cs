using Microsoft.EntityFrameworkCore;

namespace Demo_Project.Models
{
    public class AppDbContext :DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
          : base(options)
        {
        }
        public DbSet<v_Accounts> v_Accounts { get; set; } 
    }
}
