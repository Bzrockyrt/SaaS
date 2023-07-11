using SaaS.Domain.Company;
using SaaS.Domain.PIPL;

namespace SaaS.ViewModels.SuperCompany.Company
{
    public class ConfigurationCompanyViewModel
    {
        public Domain.PIPL.Company Company { get; set; }
        public IList<CompanyFunctionnalities> HaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();
        public IList<CompanyFunctionnalities> DontHaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();
        public IList<Domain.Identity.User> CompanyUsers { get; set; } = new List<Domain.Identity.User>();
    }
}
