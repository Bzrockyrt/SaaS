using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace SaaS.ViewModels.Application.WorkHour
{
    public class CreateWorkHourViewModel
    {
        public Domain.Work.WorkHour WorkHour { get; set; }

        /*public string MorningWorkSiteId { get; set; }
        public string EveningWorkSiteId { get; set; }*/

        [ValidateNever]
        public IEnumerable<SelectListItem> WorkSitesList { get; set; }

        [ValidateNever]
        public IEnumerable<SelectListItem> WorkManagersList { get; set; }
    }
}
