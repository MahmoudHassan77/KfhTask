using ExpensesApi.Data;
using ExpensesApi.Dtos;
using ExpensesApi.Models;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Services
{
    public class ExpenseService : IExpenseService
    {
        private readonly ILogger<ExpenseService> _logger;
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public ExpenseService(ILogger<ExpenseService> logger, AppDbContext context, IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _logger = logger;
            _context = context;
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }
       

        public async Task<ExpensesDto> GetExpensesAsync(string? category, DateTime? fromDate, DateTime? toDate, int page, int pageSize)
        {
            var query = _context.Expenses.AsQueryable();
            if (!string.IsNullOrEmpty(category))
            {
                query = query.Where(e => e.Category == category);
            }
            if (fromDate.HasValue)
            {
                query = query.Where(e => e.OccurredOn >= fromDate.Value);
            }
            if (toDate.HasValue)
            {
                query = query.Where(e => e.OccurredOn <= toDate.Value);
            }
            var totalItems =await query.CountAsync();
            var expenses = await query
                .OrderByDescending(e => e.OccurredOn)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Select(e => new ExpenseDto
                (
                    e.Id,
                    e.Title,
                    e.Amount,
                    e.currency,
                    e.Category,
                    e.OccurredOn,
                    e.RowVersion,
                    null
                ))
                .ToListAsync();
            return new ExpensesDto
            (
                expenses,
                totalItems,
                page,
                pageSize
            );

        }

        public async Task<ExpenseDto> GetExpenseByIdAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return null!;
            return new ExpenseDto(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.currency,
                expense.Category,
                expense.OccurredOn,
                expense.RowVersion,
                null
            );
        }

        public async Task<ExpenseDto> CreateExpenseAsync(ExpenseDto expenseDto)
        {
            if(!AllowedCurrencies.Contains(expenseDto.Currency))
            {
                return new  ExpenseDto(-1, null!, 0, null, null!, default, null!, "Unverified currency type");
            }
            var UserId = _httpContextAccessor.HttpContext?.User?.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value;
            var expense = new Expense
            {
                Title = expenseDto.Title!,
                Amount = expenseDto.Amount,
                currency = expenseDto.Currency!,
                Category = expenseDto.Category!,
                OccurredOn = DateTime.UtcNow,
                CreatedByUserId = UserId!
            };
            await _context.Expenses.AddAsync(expense);
            await _context.SaveChangesAsync();
            return new ExpenseDto(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.currency,
                expense.Category,
                expense.OccurredOn,
                expense.RowVersion,
                null
            );
        }

        public async Task<ExpenseDto> UpdateExpenseAsync(int id, ExpenseDto expenseDto)
        {
            if(!AllowedCurrencies.Contains(expenseDto.Currency))
            {
                return new ExpenseDto(-1, null!, 0, null, null!, default, null!, "Unverified currency type");
            }
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) return null!;
            expense.Title = expenseDto.Title!;
            expense.Amount = expenseDto.Amount;
            expense.currency = expenseDto.Currency!;
            expense.Category = expenseDto.Category!;
            try
            {
                _context.Expenses.Update(expense);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return new ExpenseDto(-1, null!, 0, null, null!, default, null!, "Expense was updated by another user. Please refresh and try again.");
            }
            return new ExpenseDto(
                expense.Id,
                expense.Title,
                expense.Amount,
                expense.currency,
                expense.Category,
                expense.OccurredOn,
                expense.RowVersion,
                null
            );
        }

        public async Task DeleteExpenseAsync(int id)
        {
            var expense = await _context.Expenses.FindAsync(id);
            if (expense == null) throw new DirectoryNotFoundException();
            _context.Expenses.Remove(expense);
        }

        private string[] AllowedCurrencies => _configuration.GetSection("AllowedCurrencies").Get<string[]>() ?? new[] { "EGP", "USD", "EUR" };
    }
}
