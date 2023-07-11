using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Logistic
{
    public class ArticleImage : ModelBase
    {
        public ArticleImage() : base()
        {

        }

        [Required]
        public string ImageURL { get; set; } = string.Empty;

        public string ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        [ValidateNever]
        public virtual Article Article { get; set; }
    }
}
