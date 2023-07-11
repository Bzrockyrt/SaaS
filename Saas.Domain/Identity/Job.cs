using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Identity
{
    public class Job : ModelBase
    {
        public Job() : base()
        {

        }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        public virtual string DepartmentId { get; set; }
        [ForeignKey("DepartmentId")]
        [ValidateNever]
        public virtual Department Department { get; set; }

        public IList<User> Users { get; set; } = new List<User>();
    }
}
