using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using SaaS.DataAccess.Services;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.Data
{
    public class PIPLDeveloppementDbContext : IdentityDbContext<IdentityUser, IdentityRole, string>
    {
        private readonly IPasswordHasher<User> passwordHasher;
        private readonly TenantService tenantService;
        public readonly string ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=DB_PIPLDEVELOPPEMENT;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public PIPLDeveloppementDbContext(DbContextOptions<PIPLDeveloppementDbContext> options) : base(options)
        {
            this.passwordHasher = new PasswordHasher<User>();
        }

        public DbSet<Company> Company { get; set; }
        public DbSet<Functionnality> Functionnality { get; set; }
        public DbSet<Log> Log { get; set; }
        public DbSet<User> User { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            base.OnConfiguring(optionsBuilder);
            if (this.ConnectionString != null)
                optionsBuilder.UseSqlServer(this.ConnectionString);
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Company>().HasData(
                new Company
                {
                    CompanyCode = "PIPL0001",
                    CreatorId = string.Empty,
                    CreatedOn = DateTime.Now,
                    Description = "Entreprise PIPL Développement",
                    Email = "pierrelouisippoliti@pipl-developpement.com",
                    IsEnable = true,
                    IsSuperCompany = true,
                    Name = "PIPL Développement",
                    PhoneNumber = 0633333799,
                    PostalCode = 01380,
                    SIRET = 91911872900012,
                    State = "Saint André-de-Bâgé",
                    StreetName = "rue du Villard",
                    StreetNumber = 181,
                    TenantCode = "pipl-developpement",
                });

            var user = CreateUser();

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    CreatedBy = "Pierre-Louis IPPOLITI",
                    CreatedOn = DateTime.Now,
                    Firstname = "Pierre-Louis",
                    UserName = "PlIppoliti",
                    Email = "pierrelouisippoliti@pipl-developpement.com",
                    PasswordHash = this.passwordHasher.HashPassword(user, "M-wD3W~k9m]2"),
                    IsEnable = true,
                    IsSuperUser = true,
                    Lastname = "IPPOLITI",
                });
        }
        private User CreateUser()
        {
            try
            {
                return Activator.CreateInstance<User>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
    }
}
