using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
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

        [Required]
        [Display(Name = "N° de rue")]
        public string StreetNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Nom de rue")]
        public string StreetName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Commune")]
        public string State { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Code postal")]
        public string PostalCode { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Numéro de sécurité sociale")]
        [MinLength(15, ErrorMessage = "Le numéro de sécurité sociale doit comporter 15 chiffres")]
        [MaxLength(15, ErrorMessage = "Le numéro de sécurité sociale doit comporter 15 chiffres")]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        [Required]
        [Display(Name = "Lieu de naissance")]
        [DataType(DataType.PostalCode)]
        public string BirthLocation { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Mot de passe")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public int? DepartmentId { get; set; }
        [ForeignKey(nameof(DepartmentId))]
        [ValidateNever]
        public virtual Department? Department { get; set; }

        public int? GenderId { get; set; }
        [ForeignKey(nameof(GenderId))]
        [ValidateNever]
        public virtual Gender? Gender { get; set; }

        public IList<WorkHour> WorkHours { get; set; } = new List<WorkHour>();
    }
}
