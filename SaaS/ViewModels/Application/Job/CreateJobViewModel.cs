using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SaaS.ViewModels.Application.Job
{
    public class CreateJobViewModel
    {
        public Domain.Identity.Job Job { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> DepartmentList { get; set; }
    }
}
