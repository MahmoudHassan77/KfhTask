using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Models
{
    public class Expense
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public decimal Amount { get; set; }
        public string currency { get; set; }
        public DateTime OccurredOn { get; set; }
        public string CreatedByUserId { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
