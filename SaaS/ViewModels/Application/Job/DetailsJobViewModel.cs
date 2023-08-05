using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaaS.Domain.Company;

namespace SaaS.ViewModels.Application.Job
{
    public class DetailsJobViewModel
    {
        public Domain.Identity.Job Job { get; set; }
        public IList<CompanyFunctionnalities> HaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();
        public IList<CompanyFunctionnalities> DontHaveFunctionnalities { get; set; } = new List<CompanyFunctionnalities>();

        [ValidateNever]
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
