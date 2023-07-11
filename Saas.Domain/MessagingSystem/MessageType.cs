using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.MessagingSystem
{
    public class MessageType : ModelBase
    {
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Description { get; set; } = string.Empty;
    }
}
