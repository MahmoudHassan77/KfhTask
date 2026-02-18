using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Auth
{
    public record RegisterRequest(
        [Required] [MinLength(3)] [MaxLength(150)]string Username,
        [Required][MinLength(3)][MaxLength(150)] string Password
    );
}
