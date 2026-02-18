namespace ExpensesApi.Dtos
{
    public record ExpenseDto(
        int Id,
        string? Title,
        decimal Amount,
        string? Currency,
        string? Category,
        DateTime? Date,
        byte[]? RowVersion,
        string? Message
    );
}
