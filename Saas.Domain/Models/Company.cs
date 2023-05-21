using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class Company : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(1, ErrorMessage = "Le nom de l'entreprise doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de l'entreprise ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        [MinLength(1, ErrorMessage = "La description de l'entreprise doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "La description de l'entreprise ne peut comporter plus de 25 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse")]
        [DataType(DataType.PostalCode)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "SIRET")]
        [MinLength(14, ErrorMessage = "Le numéro de SIRET de l'entreprise doit comporter 14 caractères")]
        [MaxLength(14, ErrorMessage = "Le numéro de SIRET de l'entreprise doit comporter 14 caractères")]
        public string SIRET { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Numéro de téléphone")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Code de l'entreprise")]
        [MinLength(8, ErrorMessage = "Le code de l'entreprise doit comporter 8 caractères")]
        [MaxLength(8, ErrorMessage = "Le code de l'entreprise doit comporter 8 caractères")]
        public string CompanyCode { get; set; } = string.Empty;

        public string Company_Tenant_Description { get; set; } = string.Empty;

        /*public int SubscriptionId { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        [ValidateNever]
        public virtual Subscription Subscription { get; set; }*/

        /*public int TenantId { get; set; }
        [ForeignKey(nameof(TenantId))]
        [ValidateNever]
        public virtual Tenant Tenant { get; set; }*/

        /*public int CompanySettingId { get; set; }
        [ForeignKey(nameof(CompanySettingId))]
        [ValidateNever]
        public virtual CompanySetting CompanySetting { get; set; }*/

        public IList<Department> Departments { get; set; } = new List<Department>();
    }
}
