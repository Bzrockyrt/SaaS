using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
{
    public class WorkHourImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public int WorkHourId { get; set; }
        [ForeignKey(nameof(WorkHourId))]
        [ValidateNever]
        public virtual WorkHour WorkHour { get; set; }
    }
}
