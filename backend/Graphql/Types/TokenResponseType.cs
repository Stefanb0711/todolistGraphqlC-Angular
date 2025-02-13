namespace todListBackend.Graphql.Types;

public class TokenResponseType
{
    public string Message { get; set; }
    public bool Success { get; set; }
    public string? Token { get; set; }
}