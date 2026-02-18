namespace ExpensesApi.Dtos
{
    public record ExpensesDto(
        List<ExpenseDto> Expenses,
        int TotalCount,
        int Page,
        int PageSize
        );
}
