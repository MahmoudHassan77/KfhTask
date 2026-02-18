using System.ComponentModel.DataAnnotations;

namespace ExpensesApi.Auth
{
    public record LoginRequist(
        [Required] string Username,
        [Required] string Password
    );
    
}
