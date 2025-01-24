using GraphQL.Types;

namespace todListBackend.Graphql.Types;

public class LoginInput: InputObjectGraphType
{
    public LoginInput()
    {
        Field<StringGraphType>("usernameOrEmail");
        Field<StringGraphType>("password");
    }
}