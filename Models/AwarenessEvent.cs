using System.ComponentModel.DataAnnotations;

namespace AidHopePlus.Models
{
    public class AwarenessEvent
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(150)]
        public string EventName { get; set; } = string.Empty;

        [MaxLength(150)]
        public string? Location { get; set; }

        public DateTime EventDate { get; set; }

        [MaxLength(500)]
        public string? Description { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
