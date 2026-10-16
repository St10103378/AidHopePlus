using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AidHopePlus.Models
{
    public class AttendanceRecord
    {
        [Key]
        public int Id { get; set; }

        public int? AwarenessEventId { get; set; }

        [ForeignKey(nameof(AwarenessEventId))]
        public AwarenessEvent? AwarenessEvent { get; set; }

        public int? BeneficiaryId { get; set; }

        [ForeignKey(nameof(BeneficiaryId))]
        public Beneficiary? Beneficiary { get; set; }

        [MaxLength(100)]
        public string? AttendeeName { get; set; } // used if not linked to a beneficiary

        public DateTime AttendanceDate { get; set; } = DateTime.Today;

        [MaxLength(50)]
        public string Status { get; set; } = "Present"; // Present, Absent, Late

        [MaxLength(300)]
        public string? Notes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
