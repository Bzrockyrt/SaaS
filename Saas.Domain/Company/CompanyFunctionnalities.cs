using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Company
{
    public class CompanyFunctionnalities : ModelBase
    {
        public CompanyFunctionnalities()
        {
            if (string.IsNullOrEmpty(Id))
                Id = Guid.NewGuid().ToString();
        }

        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom de la fonctionnalité doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de la fonctionnalité ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nom normalisé")]
        [MinLength(5, ErrorMessage = "Le nom normalisé doit contenir au moins 3 caractères")]
        [MaxLength(50, ErrorMessage = "Le nom normalisé ne peut contenir plus de 50 catactères")]
        public string NormalizedName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Code")]
        public string Code { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description de la fonctionnalité doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description de la fonctionnalité ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;
    }
}
