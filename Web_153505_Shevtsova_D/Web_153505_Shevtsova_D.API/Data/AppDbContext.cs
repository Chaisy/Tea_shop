using Microsoft.EntityFrameworkCore;
using Web_153505_Shevtsova_D.Domain.Entities;

namespace Web_153505_Shevtsova_D.API.Data
{
    public class AppDbContext : DbContext
    {
        public DbSet<Tea> teas {get; set;}
        public DbSet<TeaBasesCategory> basesType {get; set;}
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
    }
}
