using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AidHopePlus.Models
{
    public class SupportSession
    {
        [Key]
        public int Id { get; set; }

        public int BeneficiaryId { get; set; }

        [ForeignKey(nameof(BeneficiaryId))]
        public Beneficiary? Beneficiary { get; set; }

        [Required]
        [MaxLength(100)]
        public string SupportType { get; set; } = string.Empty; 
        // Psychosocial Support, Counselling, Medical Referral, Spiritual Care, Home Visit, Treatment Adherence Support, Food Parcel

        public DateTime SessionDate { get; set; }

        [MaxLength(500)]
        public string? FollowUpNotes { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
