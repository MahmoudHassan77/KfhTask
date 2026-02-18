using ExpensesApi.Dtos;
using ExpensesApi.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ExpensesApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ExpenseController : ControllerBase
    {
        private readonly ILogger<ExpenseController> _logger;
        private readonly IExpenseService _expenseService;

        public ExpenseController(ILogger<ExpenseController> logger, IExpenseService expenseService)
        {
            _logger = logger;
            _expenseService = expenseService;
        }




        [HttpGet]
        public async Task<IActionResult> GetExpenses(
            [FromQuery] string? category = null,
            [FromQuery] DateTime? fromDate = null,
            [FromQuery] DateTime? toDate = null,
            [FromQuery] int page = 1,
            [FromQuery] int pageSize = 10
            )
        {
            var result = await _expenseService.GetExpensesAsync(category, fromDate, toDate, page, pageSize);
            _logger.LogInformation("Retrieved {Count} expenses", result.Expenses.Count);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetExpenseById(int id)
        {
            var result = await _expenseService.GetExpenseByIdAsync(id);
            if (result == null) {
                _logger.LogInformation("Expense with id {Id} not found", id);
                return NotFound();
            }
            _logger.LogInformation("Expense with id {Id} found", id);
            return Ok(result);

        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> CreateExpense([FromBody] ExpenseDto expenseDto)
        {
            if(!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid expense data provided");
                return BadRequest(ModelState);
            }
            var result = await _expenseService.CreateExpenseAsync(expenseDto);
            if (result == null || result.Id == -1) {
                _logger.LogError("Failed to create expense");
                return StatusCode(StatusCodes.Status500InternalServerError, "Failed to create expense");
            }
            _logger.LogInformation("Created expense with id {Id}", result.Id);
            return CreatedAtAction(nameof(GetExpenseById), new { id = result.Id }, result);
        }

        [HttpPut("{id}")]
        [Authorize]
        public async Task<IActionResult> UpdateExpense(int id, [FromBody] ExpenseDto expenseDto)
        {
            if (!ModelState.IsValid)
            {
                _logger.LogWarning("Invalid expense data provided for update");
                return BadRequest(ModelState);
            }
           
                var result = await _expenseService.UpdateExpenseAsync(id, expenseDto);
                if(result == null || result.Id == -1)
                {
                    _logger.LogInformation("Expense with id {Id} not found for update", id);
                    return NotFound();
            }


            _logger.LogInformation("Updated expense with id {Id}", id);
                return Ok(result);
            
        }

        [HttpDelete("{id}")]
        [Authorize]
        public async Task<IActionResult> DeleteExpense(int id)
        {
            try
            {
                await _expenseService.DeleteExpenseAsync(id);
                _logger.LogInformation("Deleted expense with id {Id}", id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                _logger.LogInformation("Expense with id {Id} not found for deletion", id);
                return NotFound();
            }
        }
    }
}
