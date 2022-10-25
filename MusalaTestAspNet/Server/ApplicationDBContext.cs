using Microsoft.EntityFrameworkCore;
using MusalaTestAspNet.Shared.Models;

namespace MusalaTestAspNet.Server
{
    public class ApplicationDBContext:DbContext
    {
        public DbSet<Gateway> Gateways { get; set; }
        public DbSet<Peripheral> Peripherals { get; set; }
        public ApplicationDBContext(DbContextOptions<ApplicationDBContext> options)
          : base(options)
        {

        }
    }
}
