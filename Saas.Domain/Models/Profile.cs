using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Xml.Linq;

namespace SaaS.Domain.Models
{
    public class Profile : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom du profil doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du profil ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description du profil doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description du profil ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        /*public int AssociationId { get; set; }
        [ForeignKey(nameof(AssociationId))]
        [ValidateNever]
        public virtual Association Association { get; set; }*/
    }
}
