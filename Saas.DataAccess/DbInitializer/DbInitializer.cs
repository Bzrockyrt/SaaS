using Microsoft.AspNetCore.Identity;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.IRepository;

namespace SaaS.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext context;
        private readonly IUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public DbInitializer(ApplicationDbContext context,
            IUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public void Initialize()
        {
            /*try
            {
                if (this.context.Database.GetPendingMigrations().Count() > 0)
                    this.context.Database.Migrate();
                if (!this.context.Company.Any())
                {
                    Company pipldeveloppement = new Company()
                    {
                        StreetNumber = 181,
                        StreetName = "rue du Villard",
                        PostalCode = 01380,
                        State = "Saint André-de-Bâgé",
                        CompanyCode = "0000",
                        TenantCode = "pipldeveloppement",
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Entreprise PIPL Développement dirigée par Pierre-Louis IPPOLITI",
                        Email = "pierrelouisippoliti@pipl-developpement.com",
                        IsEnable = true,
                        Name = "PIPL Développement",
                        PhoneNumber = 0633333799,
                        SIRET = 012345678910121,
                    };
                    this.unitOfWork.Company.Add(pipldeveloppement);

                    *//*Company company1 = new Company()
                    {
                        StreetNumber = "",
                        StreetName = "",
                        PostalCode = "",
                        State = "",
                        CompanyCode = "ENFI1",
                        Company_Tenant_Description = "entreprisefictive-1",
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Description de l'entreprise fictive n°1",
                        Email = "test@entreprise-fictive1.com",
                        IsEnable = true,
                        Name = "Entreprise fictive n°1",
                        PhoneNumber = "1234567890",
                        SIRET = "1234567890",
                    };
                    this.unitOfWork.Company.Add(company1);

                    Company company2 = new Company()
                    {
                        StreetNumber = "",
                        StreetName = "",
                        PostalCode = "",
                        State = "",
                        CompanyCode = "ENFI2",
                        Company_Tenant_Description = "entreprisefictive-2",
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Description de l'entreprise fictive n°2",
                        Email = "test@entreprise-fictive2.com",
                        IsEnable = true,
                        Name = "Entreprise fictive n°2",
                        PhoneNumber = "1234567890",
                        SIRET = "1234567890",
                    };
                    this.unitOfWork.Company.Add(company2);

                    Company company3 = new Company()
                    {
                        StreetNumber = "",
                        StreetName = "",
                        PostalCode = "",
                        State = "",
                        CompanyCode = "ENFI3",
                        Company_Tenant_Description = "entreprisefictive-3",
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Description de l'entreprise fictive n°3",
                        Email = "test@entreprise-fictive3.com",
                        IsEnable = true,
                        Name = "Entreprise fictive n°3",
                        PhoneNumber = "1234567890",
                        SIRET = "1234567890",
                    };
                    this.unitOfWork.Company.Add(company3);*//*

                    this.context.SaveChanges();
                }
                if (!this.context.Department.Any())
                {
                    Department RetD = new Department()
                    {
                        CompanyId = this.context.Company.FirstOrDefault(c => c.Name == "PIPL Développement").Id,
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Département de la recherche et du développement",
                        IsEnable= true,
                        Name = "Recherche et Développement",
                    };
                    this.unitOfWork.Department.Add(RetD);

                    Department Logistique = new Department()
                    {
                        CompanyId = this.context.Company.FirstOrDefault(c => c.Name == "PIPL Développement").Id,
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Département de la logistique",
                        IsEnable = true,
                        Name = "Logistique",
                    };
                    this.unitOfWork.Department.Add(Logistique);

                    Department Montage = new Department()
                    {
                        CompanyId = this.context.Company.FirstOrDefault(c => c.Name == "PIPL Développement").Id,
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Département du montage",
                        IsEnable = true,
                        Name = "Montage",
                    };
                    this.unitOfWork.Department.Add(Montage);

                    Department Etudes = new Department()
                    {
                        CompanyId = this.context.Company.FirstOrDefault(c => c.Name == "PIPL Développement").Id,
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Département des études",
                        IsEnable = true,
                        Name = "Etudes",
                    };
                    this.unitOfWork.Department.Add(Etudes);

                    this.context.SaveChanges();
                }
                if (!roleManager.RoleExistsAsync("Administration").GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole("Opérateur")).GetAwaiter().GetResult();
                    roleManager.CreateAsync(new IdentityRole("Monteur")).GetAwaiter().GetResult();
                    roleManager.CreateAsync(new IdentityRole("Développeur")).GetAwaiter().GetResult();
                    roleManager.CreateAsync(new IdentityRole("Administration")).GetAwaiter().GetResult();

                    userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "pierrelouisippoliti@pipl-developpement.com",
                        Email = "pierrelouisippoliti@pipl-developpement.com",
                        Firstname = "Pierre-Louis",
                        Lastname = "IPPOLITI",
                        PhoneNumber = "0633333799",
                        StreetNumber = "181",
                        StreetName = "rue du Villard",
                        State = "Saint André-de-Bâgé",
                        PostalCode = "01380",
                        *//*DepartmentId = this.unitOfWork.Department.Get(d => d.Name == "Logistique").Id,*//*
                    }, "Test123!").GetAwaiter().GetResult();

                    userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "johndoe@pipl-developpement.com",
                        Email = "johndoe@pipl-developpement.com",
                        Firstname = "John",
                        Lastname = "DOE",
                        PhoneNumber = "0633333799",
                        StreetNumber = "181",
                        StreetName = "rue du Villard",
                        State = "Saint André-de-Bâgé",
                        PostalCode = "01380",
                        *//*DepartmentId = this.unitOfWork.Department.Get(d => d.Name == "Montage").Id,*//*
                    }, "Test123!").GetAwaiter().GetResult();

                    userManager.CreateAsync(new ApplicationUser
                    {
                        UserName = "jackdoe@pipl-developpement.com",
                        Email = "jackdoe@pipl-developpement.com",
                        Firstname = "Jack",
                        Lastname = "DOE",
                        PhoneNumber = "0633333799",
                        StreetNumber = "181",
                        StreetName = "rue du Villard",
                        State = "Saint André-de-Bâgé",
                        PostalCode = "01380",
                        *//*DepartmentId = this.unitOfWork.Department.Get(d => d.Name == "Logistique").Id,*//*
                    }, "Test123!").GetAwaiter().GetResult();


                    ApplicationUser pierrelouis = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "pierrelouisippoliti@pipl-developpement.com");
                    userManager.AddToRoleAsync(pierrelouis, "Administration").GetAwaiter().GetResult();

                    ApplicationUser john = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "johndoe@pipl-developpement.com");
                    userManager.AddToRoleAsync(john, "Monteur").GetAwaiter().GetResult();

                    ApplicationUser jack = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "jackdoe@pipl-developpement.com");
                    userManager.AddToRoleAsync(jack, "Opérateur").GetAwaiter().GetResult();
                }
                if (!this.context.Functionnality.Any())
                {
                    Functionnality Access_Administration = new Functionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Accessibilité à la partie administration",
                        Name = "Access_Administration",
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Administration);

                    Functionnality Access_MessagingSystem = new Functionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        Description = "Accessibilité à la messagerie",
                        Name = "Access_MessagingSystem",
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_MessagingSystem);

                    this.context.SaveChanges();
                }
                if (!this.context.ApplicationRoleFunctionnality.Any())
                {
                    ApplicationRoleFunctionnality applicationRoleFunctionnality1 = new ApplicationRoleFunctionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        FunctionnalityId = this.unitOfWork.Functionnality.Get(f => f.Name == "Access_Administration").Id,
                        IsEnable = true,
                        RoleId = this.unitOfWork.ApplicationRole.Get(ar => ar.Name == "Administration").Id,
                    };
                    this.unitOfWork.ApplicationRoleFunctionnality.Add(applicationRoleFunctionnality1);

                    ApplicationRoleFunctionnality applicationRoleFunctionnality2 = new ApplicationRoleFunctionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        FunctionnalityId = this.unitOfWork.Functionnality.Get(f => f.Name == "Access_MessagingSystem").Id,
                        IsEnable = true,
                        RoleId = this.unitOfWork.ApplicationRole.Get(ar => ar.Name == "Développeur").Id,
                    };
                    this.unitOfWork.ApplicationRoleFunctionnality.Add(applicationRoleFunctionnality2);

                    ApplicationRoleFunctionnality applicationRoleFunctionnality3 = new ApplicationRoleFunctionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        FunctionnalityId = this.unitOfWork.Functionnality.Get(f => f.Name == "Access_MessagingSystem").Id,
                        IsEnable = true,
                        RoleId = this.unitOfWork.ApplicationRole.Get(ar => ar.Name == "Monteur").Id,
                    };
                    this.unitOfWork.ApplicationRoleFunctionnality.Add(applicationRoleFunctionnality3);

                    ApplicationRoleFunctionnality applicationRoleFunctionnality4 = new ApplicationRoleFunctionnality()
                    {
                        CreatedBy = "Pierre-Louis IPPOLITI",
                        CreatedOn = DateTime.Now,
                        FunctionnalityId = this.unitOfWork.Functionnality.Get(f => f.Name == "Access_MessagingSystem").Id,
                        IsEnable = true,
                        RoleId = this.unitOfWork.ApplicationRole.Get(ar => ar.Name == "Opérateur").Id,
                    };
                    this.unitOfWork.ApplicationRoleFunctionnality.Add(applicationRoleFunctionnality4);

                    this.context.SaveChanges();
                }
            }
            catch (Exception ex) { }*/

        }
    }
}
