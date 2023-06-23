using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.OTHER
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

        public int UsernameChangeLimit { get; set; } = 10;

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

        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Lieu de naissance")]
        [DataType(DataType.PostalCode)]
        public string BirthLocation { get; set; } = string.Empty;

        public virtual string JobId { get; set; }
        public virtual Job Job { get; set; }

        public virtual string EmploymentContractId { get; set; }
        public virtual EmploymentContract EmploymentContract { get; set; }

        public virtual string UserStatusId { get; set; }
        public virtual UserStatus UserStatus { get; set; }

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
