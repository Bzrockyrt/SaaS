using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Logistic
{
    public class Supplier : ModelBase
    {
        public Supplier() : base()
        {

        }

        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom du fournisseur doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom du fournisseur ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description du fournisseur doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description du fournisseur ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse mail")]
        [DataType(DataType.EmailAddress)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Numéro de téléphone")]
        [DataType(DataType.PhoneNumber)]
        public string PhoneNumber { get; set; } = string.Empty;

        [Required]
        [Display(Name = "Adresse postale")]
        [DataType(DataType.PostalCode)]
        public string Address { get; set; } = string.Empty;

        public IList<Supplier_Article> Supplier_Articles { get; set; } = new List<Supplier_Article>();
    }
}
