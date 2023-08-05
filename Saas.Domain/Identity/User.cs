using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SaaS.Domain.Identity
{
    public class User : IdentityUser
    {
        /*Constructeur pour instancier l'utilisateur lors de la création d'un élément dans l'application*/
        public User()
        {
            
        }

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

        [Display(Name = "N° de rue")]
        public string StreetNumber { get; set; } = string.Empty;

        [Display(Name = "Nom de rue")]
        public string StreetName { get; set; } = string.Empty;

        [Display(Name = "Commune")]
        public string State { get; set; } = string.Empty;

        [Display(Name = "Code postal")]
        public string PostalCode { get; set; } = string.Empty;

        [Display(Name = "Numéro de sécurité sociale")]
        public string SocialSecurityNumber { get; set; } = string.Empty;

        [Display(Name = "Date de naissance")]
        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [Display(Name = "Lieu de naissance")]
        [DataType(DataType.PostalCode)]
        public string BirthLocation { get; set; } = string.Empty;

        //Cette variable permet de savoir si cet utilisateur va apparaître dans la liste des chefs de chantiers lors du
        //renseignement des heures journalières
        public bool IsAppearingInWorkerWorkHours { get; set; } = false;

        [ValidateNever]
        public virtual string JobId { get; set; }
        [ForeignKey(nameof(JobId))]
        [ValidateNever]
        public virtual Job Job { get; set; }

        [ValidateNever]
        public virtual string EmploymentContractId { get; set; }
        [ForeignKey(nameof(EmploymentContractId))]
        [ValidateNever]
        public virtual EmploymentContract EmploymentContract { get; set; }

        [ValidateNever]
        public virtual string UserStatusId { get; set; }
        [ForeignKey(nameof(UserStatusId))]
        [ValidateNever]
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
