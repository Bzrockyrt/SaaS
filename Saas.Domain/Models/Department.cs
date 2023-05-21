using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using SaaS.Domain.Models.Account;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class Department : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom du département doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du département ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description du département doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description du département ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public virtual Company Company { get; set; }

        public IList<User> Users { get; set; } = new List<User>();
    }
}
