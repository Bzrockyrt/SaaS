using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaaS.Domain.Models;

namespace SaaS.DataAccess.Data
{
    public class ApplicationDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        /*public readonly string ConnectionString;*/
        public readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_ENF1;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        /*public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
            TenantService tenantService) : base(options)
        {
            this.ConnectionString = tenantService?.GetConnectionString();
        }*/

        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
        {

        }

        public DbSet<Article> Article { get; set; }
        public DbSet<ArticleImage> ArticleImage { get; set; }
        public DbSet<Supplier> Supplier { get; set; }
        public DbSet<Supplier_Article> Supplier_Article { get; set; }
        public DbSet<Tenant> Tenant { get; set; }
        public DbSet<WorkHour> WorkHour { get; set; }
        public DbSet<WorkHour_WorkSite> WorkHour_WorkSite { get; set; }
        public DbSet<WorkHourImage> WorkHourImage { get; set; }
        public DbSet<WorkSite> WorkSite { get; set; }
        public DbSet<WorkSiteType> WorkSiteType { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (this.ConnectionString != null)
                optionsBuilder.UseSqlServer(this.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            /*modelBuilder.Entity<Company>().HasOne<CompanyPicture>(c => c.Picture).WithOne(cp => cp.Company).OnDelete(DeleteBehavior.Cascade);*/

            /*#region QueryFilters
            modelBuilder.Entity<Article>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Company>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<CompanySetting>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Department>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Functionnality>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Gender>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Profile>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<Supplier>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<WorkHour>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<WorkSite>().HasQueryFilter(a => a.IsEnable);
            modelBuilder.Entity<WorkSiteType>().HasQueryFilter(a => a.IsEnable);
            #endregion

            modelBuilder.Entity<Article>().HasMany<ArticleImage>(a => a.ArticleImages).WithOne(ai => ai.Article).HasForeignKey(ai => ai.ArticleId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Supplier_Article>().HasKey(sa => new { sa.SupplierId, sa.ArticleId });
            modelBuilder.Entity<Supplier_Article>().HasOne<Supplier>(sa => sa.Supplier).WithMany(s => s.Supplier_Articles).HasForeignKey(sa => sa.SupplierId);
            modelBuilder.Entity<Supplier_Article>().HasOne<Article>(sa => sa.Article).WithMany(a => a.Supplier_Articles).HasForeignKey(sa => sa.ArticleId);

            modelBuilder.Entity<ApplicationUser>().HasOne<Gender>(u => u.Gender).WithMany(g => g.Users).HasForeignKey(u => u.GenderId).OnDelete(DeleteBehavior.Cascade);
            modelBuilder.Entity<ApplicationUser>().Property(u => u.BirthDate).IsRequired(false);

            modelBuilder.Entity<WorkHour>().HasOne<ApplicationUser>(wh => wh.User).WithMany(u => u.WorkHours).HasForeignKey(wh => wh.UserId).OnDelete(DeleteBehavior.NoAction);

            modelBuilder.Entity<WorkHourImage>().HasOne<WorkHour>(whi => whi.WorkHour).WithMany(wh => wh.WorkHourImages).HasForeignKey(whi => whi.WorkHourId).OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<WorkHour_WorkSite>().HasKey(whws => new { whws.WorkHourId, whws.WorkSiteId });
            modelBuilder.Entity<WorkHour_WorkSite>().HasOne<WorkHour>(whws => whws.WorkHour).WithMany(wh => wh.WorkHour_WorkSites).HasForeignKey(whws => whws.WorkHourId);
            modelBuilder.Entity<WorkHour_WorkSite>().HasOne<WorkSite>(whws => whws.WorkSite).WithMany(ws => ws.WorkHour_WorkSites).HasForeignKey(whws => whws.WorkSiteId);

            modelBuilder.Entity<WorkSiteType>().HasMany<WorkSite>(wst => wst.WorkSites).WithOne(ws => ws.WorkSiteType).HasForeignKey(ws => ws.WorkSiteTypeId).OnDelete(DeleteBehavior.Cascade);*/
        }
    }
}
