namespace ExpensesApi.Auth
{
    public record RegisterResponce(
        string? UserId,
        string? UserName,
        string Message
    );
}
