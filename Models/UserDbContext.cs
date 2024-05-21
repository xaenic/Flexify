using Flexify.Controllers;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;
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
        public DbSet<Socials> Socials { get; set; }
        public object Emails { get; internal set; }
        public DbSet<Theme> themes { get; set; }
        public DbSet<PostModel> Posts { get; set; }
    }
}
