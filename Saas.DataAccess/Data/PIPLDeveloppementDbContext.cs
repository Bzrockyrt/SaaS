using Microsoft.EntityFrameworkCore;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Data
{
    public class PIPLDeveloppementDbContext : DbContext
    {
        /*public readonly string ConnectionString;*/
        public readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_PIPLDEVELOPPEMENT;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        /*public PIPLDeveloppementDbContext(DbContextOptions<ApplicationDbContext> options,
            TenantService tenantService) : base(options)
        {
            this.ConnectionString = tenantService?.GetConnectionString();
        }*/

        public PIPLDeveloppementDbContext(DbContextOptions<PIPLDeveloppementDbContext> options) : base(options)
        {

        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Functionnality> Functionnality { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (this.ConnectionString != null)
                optionsBuilder.UseSqlServer(this.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {

        }
    }
}
