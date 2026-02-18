namespace ExpensesApi.Auth
{
    public class JwtSettngs
    {
        public const string SectionName = "Jwt";

        public string Key { get; set; }
        public string Issuer { get; set; }
        public string Audience { get; set; }
        public int ExpiryInMinutes { get; set; }
    }
}
