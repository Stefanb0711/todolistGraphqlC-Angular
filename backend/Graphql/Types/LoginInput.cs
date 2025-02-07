using GraphQL;

namespace todListBackend.Graphql.Types;

public class LoginInput
{
    public string UsernameOrEmail { get; set; }
    public string Password { get; set; }
}