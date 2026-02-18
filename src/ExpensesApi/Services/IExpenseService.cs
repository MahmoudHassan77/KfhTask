using ExpensesApi.Dtos;

namespace ExpensesApi.Services
{
    public interface IExpenseService
    {
        Task<ExpensesDto> GetExpensesAsync(string? category, DateTime? fromDate, DateTime? toDate, int page, int pageSize);
        Task<ExpenseDto?> GetExpenseByIdAsync(int id);
        Task<ExpenseDto?> CreateExpenseAsync(ExpenseDto expenseDto);
        Task<ExpenseDto?> UpdateExpenseAsync(int id, ExpenseDto expenseDto);
        Task DeleteExpenseAsync(int id);
    }
}
