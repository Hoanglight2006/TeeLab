using System.ComponentModel.DataAnnotations;

namespace TeeLab.Models
{
    public class ChatHistory
    {
        [Key]
        public int Id { get; set; }
        public string? UserId { get; set; }
        public string UserMessage { get; set; } = string.Empty;
        public string BotResponse { get; set; } = string.Empty;
        public DateTime Timestamp { get; set; } = DateTime.Now;
    }
}