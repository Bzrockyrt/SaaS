using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser>
    {
        public ApplicationDbContext()
        {
            
        }

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleImage> ArticleImage { get; set; }
        public DbSet<Company> Company { get; set; }
        public DbSet<CompanySetting> CompanySetting { get; set; }
        public DbSet<Department> Department { get; set; }
        public DbSet<Functionnality> Functionnality { get; set; }
        public DbSet<Gender> Gender { get; set; }
        public DbSet<Subscription> Subscription { get; set; }
        public DbSet<Subscription_Functionnality> Subscription_Functionnality { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Supplier_Article> Supplier_Article { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<User> User { get; set; }
        public DbSet<WorkHour> WorkHour { get; set; }
        public DbSet<WorkHour_WorkSite> WorkHour_WorkSite { get; set; }
        public DbSet<WorkHourImage> WorkHourImage { get; set; }
        public DbSet<WorkSite> WorkSite { get; set; }
        public DbSet<WorkSiteType> WorkSiteType { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*Configures one-to-many relationships*/
            modelBuilder.Entity<Article>().HasMany<ArticleImage>(a => a.ArticleImages).WithOne(ai => ai.Article).HasForeignKey(ai => ai.ArticleId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Article>().HasKey(sa => new { sa.SupplierId, sa.ArticleId });
            modelBuilder.Entity<Supplier_Article>().HasOne<Supplier>(sa => sa.Supplier).WithMany(s => s.Supplier_Articles).HasForeignKey(sa => sa.SupplierId);
            modelBuilder.Entity<Supplier_Article>().HasOne<Article>(sa => sa.Article).WithMany(a => a.Supplier_Articles).HasForeignKey(sa => sa.ArticleId);

            modelBuilder.Entity<Subscription>().HasMany<Company>(s => s.Companies).WithOne(c => c.Subscription).HasForeignKey(c => c.SubscriptionId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Company>().HasOne<Tenant>(c => c.Tenant).WithOne(t => t.Company).HasForeignKey<Tenant>(t => t.CompanyId);

            modelBuilder.Entity<Company>().HasOne<CompanySetting>(c => c.CompanySetting).WithOne(cs => cs.Company).HasForeignKey<CompanySetting>(cs => cs.CompanyId);

            modelBuilder.Entity<Company>().HasMany<Department>(c => c.Departments).WithOne(d => d.Company).HasForeignKey(d => d.CompanyId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Subscription_Functionnality>().HasKey(sf => new { sf.SubscriptionId, sf.FunctionnalityId });
            modelBuilder.Entity<Subscription_Functionnality>().HasOne<Subscription>(sf => sf.Subscription).WithMany(s => s.Subscription_Functionnalities).HasForeignKey(sf => sf.SubscriptionId);
            modelBuilder.Entity<Subscription_Functionnality>().HasOne<Functionnality>(sf => sf.Functionnality).WithMany(f => f.Subscription_Functionnalities).HasForeignKey(sf => sf.FunctionnalityId);

            modelBuilder.Entity<User>().HasOne<Gender>(u => u.Gender).WithMany(g => g.Users).HasForeignKey(u => u.GenderId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkHour>().HasOne<User>(wh => wh.User).WithMany(u => u.WorkHours).HasForeignKey(wh => wh.UserId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkHourImage>().HasOne<WorkHour>(whi => whi.WorkHour).WithMany(wh => wh.WorkHourImages).HasForeignKey(whi => whi.WorkHourId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkHour_WorkSite>().HasKey(whws => new { whws.WorkHourId, whws.WorkSiteId });
            modelBuilder.Entity<WorkHour_WorkSite>().HasOne<WorkHour>(whws => whws.WorkHour).WithMany(wh => wh.WorkHour_WorkSites).HasForeignKey(whws => whws.WorkHourId);
            modelBuilder.Entity<WorkHour_WorkSite>().HasOne<WorkSite>(whws => whws.WorkSite).WithMany(ws => ws.WorkHour_WorkSites).HasForeignKey(whws => whws.WorkSiteId);

            modelBuilder.Entity<WorkSiteType>().HasMany<WorkSite>(wst => wst.WorkSites).WithOne(ws => ws.WorkSiteType).HasForeignKey(ws => ws.WorkSiteTypeId).OnDelete(DeleteBehavior.Cascade);
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (!optionsBuilder.IsConfigured)
                optionsBuilder.UseSqlServer("Server=(localdb)\\mssqllocaldb;Database=SaaS;Trusted_Connection=True;MultipleActiveResultSets=true");
        }
    }
}
