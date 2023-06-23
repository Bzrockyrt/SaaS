using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.OTHER
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

        [ValidateNever]
        public IList<Job> Jobs { get; set; } = new List<Job>();
    }
}
