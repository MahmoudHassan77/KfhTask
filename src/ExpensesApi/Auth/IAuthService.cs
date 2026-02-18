using ExpensesApi.Models;

namespace ExpensesApi.Auth
{
    public interface IAuthService
    {
        Task<RegisterResponce?> RegisterAsync(RegisterRequest request);
        Task<LoginResponse?> LoginAsync(LoginRequist request);
        
    }
}
