using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.PIPL
{
    public class Company : PIPLModelBase
    {
        public Company() : base()
        {

        }

        [Required]
        [Display(Name = "Nom")]
        [MinLength(1, ErrorMessage = "Le nom de l'entreprise doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de l'entreprise ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        [MinLength(1, ErrorMessage = "La description de l'entreprise doit comporter au minimum 1 caractères")]
        [MaxLength(150, ErrorMessage = "La description de l'entreprise ne peut comporter plus de 150 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "N° de rue")]
        public uint StreetNumber { get; set; }

        [Required]
        [Display(Name = "Nom de rue")]
        public string StreetName { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Commune")]
        public string State { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Code postal")]
        public uint PostalCode { get; set; }

        [Required]
        [Display(Name = "SIRET")]
        /*[MinLength(14, ErrorMessage = "Le numéro de SIRET de l'entreprise doit comporter 14 caractères")]
        [MaxLength(14, ErrorMessage = "Le numéro de SIRET de l'entreprise doit comporter 14 caractères")]*/
        public double SIRET { get; set; }

        [Required]
        [Display(Name = "Adresse mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Numéro de téléphone")]
        [DataType(DataType.PhoneNumber)]
        public double PhoneNumber { get; set; }

        [Required]
        [Display(Name = "Code de l'entreprise")]
        [MinLength(8, ErrorMessage = "Le code de l'entreprise doit comporter 8 caractères")]
        [MaxLength(8, ErrorMessage = "Le code de l'entreprise doit comporter 8 caractères")]
        public string CompanyCode { get; set; } = string.Empty;

        public string TenantCode { get; set; } = string.Empty;

        public bool IsSuperCompany { get; set; } = false;
    }
}
