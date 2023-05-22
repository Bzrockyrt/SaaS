using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SaaS.DataAccess.Data
{
    public class SuperAdminDbContext : IdentityDbContext<User>
    {
        public DbSet<Company> Company { get; set; }
        public DbSet<Tenant> Tenant { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultScheme("superadmin");
        }
    }
}
