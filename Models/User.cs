using System.ComponentModel.DataAnnotations;

namespace AidHopePlus.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(50)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [MaxLength(100)]
        public string PasswordHash { get; set; } = string.Empty; // In real app use proper hashing

        [MaxLength(100)]
        public string? FullName { get; set; }
        public string? Email { get; set; }


        [MaxLength(50)]
        public string Role { get; set; } = "Staff"; // Staff, Admin
    }
}
