using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;
using SaaS.Domain.Company;

namespace SaaS.ViewModels.Application.User
{
    public class DetailsUserViewModel
    {
        public Domain.Identity.User User { get; set; }

        public virtual string JobId { get; set; } = string.Empty;

        [ValidateNever]
        public IEnumerable<SelectListItem> SubsidiaryList { get; set; }
    }
}
