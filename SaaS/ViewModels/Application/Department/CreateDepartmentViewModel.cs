using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SaaS.ViewModels.Application.Department
{
    public class CreateDepartmentViewModel
    {
        public Domain.Identity.Department Department { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> SubsidiaryList { get; set; }
    }
}
