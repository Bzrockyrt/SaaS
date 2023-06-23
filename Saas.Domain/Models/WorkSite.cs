using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SaaS.Domain.Models
{
    public class WorkSite : ModelBase
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(1, ErrorMessage = "Le nom du chantier doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du chantier ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Description")]
        [MinLength(1, ErrorMessage = "La description du chantier doit comporter au minimum 1 caractères")]
        [MaxLength(25, ErrorMessage = "La description du chantier ne peut comporter plus de 25 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse")]
        [DataType(DataType.PostalCode)]
        public string Address { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Date de début")]
        [DataType(DataType.Date)]
        public DateTime StartDate { get; set; }

        [Required]
        [Display(Name = "Date de fin")]
        [DataType(DataType.Date)]
        public DateTime EndDate { get; set; }

        [Required]
        [Display(Name = "Commentaire")]
        [StringLength(50, ErrorMessage = "Le commentaire doit faire 50 caractères")]
        public string Comment { get; set; } = string.Empty;

        /*public string CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public virtual Company Company { get; set; }*/

        public string WorkSiteTypeId { get; set; }
        [ForeignKey(nameof(WorkSiteTypeId))]
        [ValidateNever]
        public virtual WorkSiteType WorkSiteType { get; set; }

        public IList<WorkHour_WorkSite> WorkHour_WorkSites { get; set; } = new List<WorkHour_WorkSite>();
    }
}
