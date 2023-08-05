using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SaaS.DataAccess.Data;
using SaaS.DataAccess.Repository.PIPL.IRepository;
using SaaS.Domain.PIPL;

namespace SaaS.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly ApplicationDbContext applicationDbContext;
        private readonly PIPLDeveloppementDbContext piplDeveloppementDbContext;
        private readonly ISuperCompanyUnitOfWork unitOfWork;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly UserManager<IdentityUser> userManager;

        public DbInitializer(ApplicationDbContext applicationDbContext,
            PIPLDeveloppementDbContext piplDeveloppementDbContext,
            ISuperCompanyUnitOfWork unitOfWork,
            RoleManager<IdentityRole> roleManager,
            UserManager<IdentityUser> userManager)
        {
            this.applicationDbContext = applicationDbContext;
            this.piplDeveloppementDbContext = piplDeveloppementDbContext;
            this.unitOfWork = unitOfWork;
            this.roleManager = roleManager;
            this.userManager = userManager;
        }

        public void InitializePIPLDeveloppementDb()
        {
            try
            {
                if (this.piplDeveloppementDbContext.Database.GetPendingMigrations().Count() > 0)
                    this.piplDeveloppementDbContext.Database.Migrate();
                if (!this.piplDeveloppementDbContext.Company.Any())
                {
                    Company pipldeveloppement = new Company()
                    {
                        StreetNumber = 181,
                        StreetName = "rue du Villard",
                        PostalCode = 01380,
                        State = "Saint André-de-Bâgé",
                        CompanyCode = "0000",
                        TenantCode = "pipldeveloppement",
                        CreatorId = string.Empty,
                        CreatedOn = DateTime.Now,
                        Description = "Entreprise PIPL Développement dirigée par Pierre-Louis IPPOLITI",
                        Email = "pierrelouisippoliti@pipl-developpement.com",
                        IsEnable = true,
                        Name = "PIPL Développement",
                        PhoneNumber = 0633333799,
                        SIRET = 012345678910121,
                    };
                    this.unitOfWork.Company.Add(pipldeveloppement);

                    /*Company company1 = new Company()
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
                    this.unitOfWork.Company.Add(company3); */

                    this.piplDeveloppementDbContext.SaveChanges();
                }
                if (!roleManager.RoleExistsAsync("Administration").GetAwaiter().GetResult())
                {
                    roleManager.CreateAsync(new IdentityRole("Administration")).GetAwaiter().GetResult();

                    userManager.CreateAsync(new Domain.PIPL.User
                    {
                        UserName = "pierrelouisippoliti@pipl-developpement.com",
                        Email = "pierrelouisippoliti@pipl-developpement.com",
                        Firstname = "Pierre-Louis",
                        Lastname = "IPPOLITI",
                        PhoneNumber = "0633333799",
                    }, "M-wD3W~k9m]2").GetAwaiter().GetResult();

                    /*userManager.CreateAsync(new ApplicationUser
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
                    }, "Test123!").GetAwaiter().GetResult();s


                    ApplicationUser pierrelouis = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "pierrelouisippoliti@pipl-developpement.com");
                    userManager.AddToRoleAsync(pierrelouis, "Administration").GetAwaiter().GetResult();

                    ApplicationUser john = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "johndoe@pipl-developpement.com");
                    userManager.AddToRoleAsync(john, "Monteur").GetAwaiter().GetResult();

                    ApplicationUser jack = this.context.ApplicationUser.FirstOrDefault(u => u.Email == "jackdoe@pipl-developpement.com");
                    userManager.AddToRoleAsync(jack, "Opérateur").GetAwaiter().GetResult();*/
                }
                if (!this.piplDeveloppementDbContext.Functionnality.Any())
                {
                    #region Access
                    Functionnality Access_SuperAdministration = new Functionnality()
                    {
                        Name = "Accès super administration",
                        NormalizedName = "Access_SuperAdministration",
                        Description = "Accès à la partie PIPL Développement",
                        Code = "F-A-SUPERADMIN",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_SuperAdministration);

                    Functionnality Access_Dashboard = new Functionnality()
                    {
                        Name = "Accès au tableau de bord",
                        NormalizedName = "Access_Dashboard",
                        Description = "Accès au tableau de bord de l'employé",
                        Code = "F-A-TABLEAUDEBORD",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_SuperAdministration);

                    Functionnality Access_Administration = new Functionnality()
                    {
                        Name = "Accès administration",
                        NormalizedName = "Access_Administration",
                        Description = "Accès à la partie administration de l'entreprise",
                        Code = "F-A-ADMINISTRATION",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Administration);

                    Functionnality Access_MessagingSystem = new Functionnality()
                    {
                        Name = "Accès messagerie",
                        NormalizedName = "Access_MessagingSystem",
                        Description = "Accès à la messagerie de l'utilisateur",
                        Code = "F-A-MESSAGERIE",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_MessagingSystem);

                    Functionnality Access_WorkSites = new Functionnality()
                    {
                        Name = "Accès chantiers",
                        NormalizedName = "Access_WorkSites",
                        Description = "Accès aux chantiers de l'entreprise",
                        Code = "F-A-CHANTIERS",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_WorkSites);

                    Functionnality Access_Users = new Functionnality()
                    {
                        Name = "Accès employés",
                        NormalizedName = "Access_CompanyUsers",
                        Description = "Accès aux employés de l'entreprise",
                        Code = "F-A-EMPLOYES",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Users);

                    Functionnality Access_Jobs = new Functionnality()
                    {
                        Name = "Accès postes",
                        NormalizedName = "Access_Jobs",
                        Description = "Accès aux postes de l'entreprise",
                        Code = "F-A-JOBS",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Jobs);

                    Functionnality Access_Departments = new Functionnality()
                    {
                        Name = "Accès services",
                        NormalizedName = "Access_Departments",
                        Description = "Accès aux services de l'entreprise",
                        Code = "F-A-SERVICES",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Departments);

                    Functionnality Access_Subsidiaries = new Functionnality()
                    {
                        Name = "Accès filliales",
                        NormalizedName = "Access_Subsidiaries",
                        Description = "Accès aux filliales de l'entreprise",
                        Code = "F-A-FILLIALES",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_Subsidiaries);

                    Functionnality Access_WorkHours = new Functionnality()
                    {
                        Name = "Accès heures journalières personnelles",
                        NormalizedName = "Access_WorkHours",
                        Description = "Accès aux heures journalières personnelles",
                        Code = "F-A-HEURESJOURPERSO",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_WorkHours);

                    Functionnality Access_GlobalWorkHours = new Functionnality()
                    {
                        Name = "Accès heures journalières globales",
                        NormalizedName = "Access_GlobalWorkHours",
                        Description = "Accès aux heures journalières globales des employés",
                        Code = "F-A-HEURESJOURGLOBALES",
                        CreatorId = "",
                        CreatedOn = DateTime.Now,
                        IsEnable = true,
                    };
                    this.unitOfWork.Functionnality.Add(Access_GlobalWorkHours);
                    #endregion
                    #region Create
                    #endregion

                    this.piplDeveloppementDbContext.SaveChanges();
                }
                /*if (!this.piplDeveloppementDbContext.ApplicationRoleFunctionnality.Any())
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
                }*/
            }
            catch (Exception ex) { }
        }

        public void InitializeApplicationDb()
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
