namespace SaaS.Domain.Models.MessagingSystem
{
    public class ConversationMessage
    {
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        public int ConversationId { get; set; }
        public virtual Conversation Conversation { get; set; }

        public int SenderId { get; set; }
        public virtual User Sender { get; set; }
    }
}
