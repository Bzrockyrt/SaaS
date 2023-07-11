using System.ComponentModel.DataAnnotations;

namespace SaaS.Domain.Company
{
    public class Log : ModelBase
    {
        public Log() : base()
        {

        }

        [Required]
        public string ExceptionName { get; set; } = string.Empty;

        [Required]
        public string Message { get; set; } = string.Empty;

        [Required]
        public string Source { get; set; } = string.Empty;

        [Required]
        public string DevNote { get; set; } = string.Empty;

        [Required]
        public LogType LogType { get; set; }
    }
}
