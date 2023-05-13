using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
{
    public class Subscription : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom de la souscription doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de la souscription ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description de la souscription doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description de la souscription ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        public List<Subscription_Functionnality> Subscription_Functionnalities { get; set; } = new List<Subscription_Functionnality>();

        public IList<Company> Companies { get; set; } = new List<Company>();
    }
}
