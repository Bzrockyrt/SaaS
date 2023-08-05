using SaaS.Domain.Company;

namespace SaaS.ViewModels.SuperCompany.Company
{
    public class DetailsCompanyViewModel
    {
        public Domain.PIPL.Company Company { get; set; }
        public IList<CompanyFunctionnalities> HaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();
        public IList<CompanyFunctionnalities> DontHaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();
    }
}
