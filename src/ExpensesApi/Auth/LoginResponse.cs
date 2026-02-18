namespace ExpensesApi.Auth
{
    public record LoginResponse(
        string? Token,
        string? UserId,
        string? UserName,
        string Message
    );
}
