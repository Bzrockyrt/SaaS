using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.MessagingSystem
{
    public class Message : ModelBase
    {
        [Key]
        public string Id { get; set; }

        [Required]
        public string Subject { get; set; } = string.Empty;

        [Required]
        public string Body { get; set; } = string.Empty;

        public DateTime WritingOn { get; set; } = DateTime.Now;

        public DateTime ReadedOn { get; set; }
        public bool IsRead => ReadedOn != DateTime.MinValue;

        public string? MessageTypeId { get; set; }
        public virtual MessageType? MessageType { get; set; }

        public string? MessagePriorityId { get; set; }
        public virtual MessagePriority? MessagePriority { get; set; }

        public string SenderId { get; set; }
        /*public virtual ApplicationUser Sender { get; set; }*/

        public string RecipientId { get; set; }
        /*public virtual ApplicationUser Recipient { get; set; }*/

        public ICollection<Attachment> Attachments { get; set; }
    }
}
