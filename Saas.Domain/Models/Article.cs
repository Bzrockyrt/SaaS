using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Models
{
    public class Article : BaseModel
    {
        [Required]
        [Display(Name = "Nom")]
        [MinLength(3, ErrorMessage = "Le nom de l'article doit comporter au minimum 3 caractères")]
        [MaxLength(25, ErrorMessage = "Le nom de l'article ne peut comporter plus de 25 caractères")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Description")]
        [MinLength(5, ErrorMessage = "La description de l'article doit comporter au minimum 5 caractères")]
        [MaxLength(100, ErrorMessage = "La description de l'article ne peut comporter plus de 100 caractères")]
        public string Description { get; set; } = string.Empty;

        [Display(Name = "Code article")]
        [MinLength(5, ErrorMessage = "Le code articledoit comporter au minimum 5 caractères")]
        [MaxLength(10, ErrorMessage = "Le code article ne peut comporter plus de 100 caractères")]
        public string CodeArticle { get; set; } = string.Empty;

        public IList<Supplier_Article> Supplier_Articles { get; set; } = new List<Supplier_Article>();

        [ValidateNever]
        public IList<ArticleImage> ArticleImages { get; set; } = new List<ArticleImage>();

        /*public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public virtual Company Company { get; set; }*/
    }
}
