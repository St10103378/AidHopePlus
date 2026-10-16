using System.ComponentModel.DataAnnotations;

namespace AidHopePlus.Models
{
    public class Beneficiary
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(20)]
        public string BeneficiaryCode { get; set; } = string.Empty; // e.g. B001

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; } = string.Empty;

        [MaxLength(20)]
        public string? ContactNumber { get; set; }

        [MaxLength(50)]
        public string Status { get; set; } = "HIV Positive"; // HIV Positive / Affected

        public DateTime DateRegistered { get; set; } = DateTime.UtcNow;

        public string? Notes { get; set; }

        // Navigation
        public ICollection<SupportSession> SupportSessions { get; set; } = new List<SupportSession>();
    }
}
