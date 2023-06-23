using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class Supplier_Article : ModelBase
    {
        public string SupplierId { get; set; }
        [ForeignKey(nameof(SupplierId))]
        [ValidateNever]
        public virtual Supplier Supplier { get; set; }

        public string ArticleId { get; set; }
        [ForeignKey(nameof(ArticleId))]
        [ValidateNever]
        public virtual Article Article { get; set; }
    }
}
