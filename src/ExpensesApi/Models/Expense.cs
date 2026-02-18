using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Models
{
    public class Expense
    {
        public int Id { get; set; }
        [Required]
        [MaxLength(300)]
        public string Title { get; set; } = String.Empty;
        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal Amount { get; set; }
        [Required]
        [MaxLength(3)]
        public string currency { get; set; } = String.Empty;
        [Required]
        [MaxLength(150)]
        public string Category { get; set; } = String.Empty;
        [Required]
        public DateTime OccurredOn { get; set; }
        [Required]
        public User? CreatedByUserId { get; set; } 
        [Timestamp]
        public byte[]? RowVersion { get; set; }
    }
}
