using Microsoft.EntityFrameworkCore;
namespace Flexify.Models
{
    public class FlexifyDbContext : DbContext
    {
        public FlexifyDbContext(DbContextOptions<FlexifyDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
    }
   
}
