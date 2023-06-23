using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class WorkHour_WorkSite : ModelBase
    {
        public string WorkHourId { get; set; }
        [ForeignKey(nameof(WorkHourId))]
        [ValidateNever]
        public virtual WorkHour WorkHour { get; set; }

        public string WorkSiteId { get; set; }
        [ForeignKey(nameof(WorkSiteId))]
        [ValidateNever]
        public virtual WorkSite WorkSite { get; set; }
    }
}
