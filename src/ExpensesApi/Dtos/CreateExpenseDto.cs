using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Dtos
{
    public record CreateExpenseDto(
        [Required][MaxLength(300)] string Title,
        [Required][Range(0.01, double.MaxValue)] decimal Amount,
        [Required] string Currency,
        [Required][MaxLength(150)] string Category
        );
}
