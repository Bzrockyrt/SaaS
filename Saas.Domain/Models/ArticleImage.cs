using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class ArticleImage
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string ImageURL { get; set; }

        public int ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        [ValidateNever]
        public virtual Article Article { get; set; }
    }
}
