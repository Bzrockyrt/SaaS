using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations.Schema;

namespace SaaS.Domain.Models
{
    public class Subscription_Functionnality : BaseModel
    {
        public int SubscriptionId { get; set; }
        [ForeignKey(nameof(SubscriptionId))]
        [ValidateNever]
        public virtual Subscription Subscription { get; set; }

        public int FunctionnalityId { get; set; }
        [ForeignKey(nameof(FunctionnalityId))]
        [ValidateNever]
        public virtual Functionnality Functionnality { get; set; }
    }
}
