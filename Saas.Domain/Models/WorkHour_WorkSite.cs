using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class WorkHour_WorkSite : BaseModel
    {
        public int WorkHourId { get; set; }
        [ForeignKey(nameof(WorkHourId))]
        [ValidateNever]
        public virtual WorkHour WorkHour { get; set; }

        public int WorkSiteId { get; set; }
        [ForeignKey(nameof(WorkSiteId))]
        [ValidateNever]
        public virtual WorkSite WorkSite { get; set; }
    }
}
