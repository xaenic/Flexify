using Flexify.Controllers;
using Microsoft.EntityFrameworkCore;
namespace Flexify.Models
{
    public class FlexifyDbContext : DbContext
    {
        public FlexifyDbContext()
        {
        }

        public FlexifyDbContext(DbContextOptions<FlexifyDbContext> options) : base(options)
        {

        }

        public DbSet<UserModel> Users { get; set; }
        public DbSet<Email> email { get; set; }
        public object Emails { get; internal set; }
        public DbSet<Theme> themes { get; set; }
    }
}
