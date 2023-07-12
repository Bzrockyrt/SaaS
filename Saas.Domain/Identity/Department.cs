using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Identity
{
    public class Department : ModelBase
    {
        public Department() : base()
        {

        }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        public virtual string SubsidiaryId { get; set; }
        [ForeignKey("SubsidiaryId")]
        [ValidateNever]
        public virtual Subsidiary Subsidiary { get; set; }

        [ValidateNever]
        public IList<Job> Jobs { get; set; } = new List<Job>();
    }
}
