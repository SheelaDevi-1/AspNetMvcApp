using Automobile_Speed.Models;
using Microsoft.EntityFrameworkCore;

namespace Automobile_Speed.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }
        public DbSet<Brand> Brands { get; set; }
        
             
        
    }
}
