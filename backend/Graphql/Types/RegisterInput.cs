using GraphQL.Types;

namespace todListBackend.Graphql.Types;

public class RegisterInput: ObjectGraphType
{
    public RegisterInput()
    {
        Field<StringGraphType>("username");
        Field<StringGraphType>("email");
        Field<StringGraphType>("password");
        Field<StringGraphType>("passwordConfirm");

    }
    
}