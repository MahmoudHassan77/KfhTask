using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Models
{
    public class User
    {
        public string Id { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserId { get; set; } = String.Empty;
        [Required]
        [MaxLength(150)]
        public string UserName { get; set; } = String.Empty;
        [Required]
        [MaxLength(150)]
        public string PasswordHash { get; set; } = String.Empty;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    }
}
