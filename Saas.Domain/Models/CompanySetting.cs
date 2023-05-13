using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class CompanySetting : BaseModel
    {
        public int CompanyId { get; set; }
        [ForeignKey(nameof(CompanyId))]
        [ValidateNever]
        public virtual Company Company { get; set; }
    }
}
