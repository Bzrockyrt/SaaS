using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Identity
{
    public class Subsidiary : ModelBase
    {
        public Subsidiary() : base()
        {
            
        }

        [Required]
        public string Name { get; set; } = string.Empty;

        [Required]
        public string Description { get; set; } = string.Empty;

        [Required]
        public string Code { get; set; } = string.Empty;

        public string PrimaryColor { get; set; } = "";

        public string SecondaryColor { get; set; } = "";

        public string TertiaryColor { get; set; } = "";

        [ValidateNever]
        public IList<Department> Departments { get; set; } = new List<Department>();
    }
}
