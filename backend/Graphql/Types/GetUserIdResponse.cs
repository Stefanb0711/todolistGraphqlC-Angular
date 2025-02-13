namespace todListBackend.Graphql.Types;

public class GetUserIdResponse
{
    public string? UserId { get; set; }
    public string? Message { get; set; }
    public bool? Success { get; set; }
}