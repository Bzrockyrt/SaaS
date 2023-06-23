namespace SaaS.Domain.Models.MessagingSystem
{
    public class Attachment : ModelBase
    {
        public string FileName { get; set; } = string.Empty;

        public byte[] Data { get; set; }

        public string Message { get; set; } = string.Empty;
    }
}
