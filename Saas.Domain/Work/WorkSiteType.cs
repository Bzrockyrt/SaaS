using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace SaaS.Domain.Work
{
    public class WorkSiteType : ModelBase
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom du type de chantier doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du type de chantier ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description du type de chantier doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description du type de chantier ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        public IList<WorkSite> WorkSites { get; set; } = new List<WorkSite>();
    }
}
