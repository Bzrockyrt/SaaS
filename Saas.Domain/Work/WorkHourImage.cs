using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Work
{
    public class WorkHourImage : ModelBase
    {
        public WorkHourImage() : base()
        {

        }

        [Required]
        public string ImageURL { get; set; } = string.Empty;

        public string WorkHourId { get; set; }
        [ForeignKey(nameof(WorkHourId))]
        [ValidateNever]
        public virtual WorkHour WorkHour { get; set; }
    }
}
