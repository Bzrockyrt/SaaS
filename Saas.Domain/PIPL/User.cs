using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;

namespace SaaS.Domain.PIPL
{
    public class User : IdentityUser
    {
        [Required]
        [Display(Name = "Prénom")]
        [MinLength(1, ErrorMessage = "Le prénom de l'utilisateur doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "Le prénom de l'utilisateur ne peut comporter plus de 25 caractères")]
        public string Firstname { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nom")]
        [MinLength(1, ErrorMessage = "Le nom de l'utilisateur doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de l'utilisateur ne peut comporter plus de 25 caractères")]
        public string Lastname { get; set; } = string.Empty;

        [NotMapped]
        [Display(Name = "Nom complet")]
        public string Fullname
        {
            get { return Firstname + " " + Lastname; }
        }

        public bool IsSuperUser { get; set; }

        [Display(Name = "Créateur")]
        public string CreatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de création")]
        public DateTime CreatedOn { get; set; } = DateTime.Now;

        [Display(Name = "Modificateur")]
        public string UpdatedBy { get; set; } = string.Empty;

        [Display(Name = "Date et heure de modification")]
        public DateTime? UpdatedOn { get; set; }

        public bool IsEnable { get; set; } = true;
    }
}
