using GraphQL.Types;

namespace todListBackend.Graphql.Types;

public class LoginResponseType : ObjectGraphType
{
    
    public LoginResponseType()
    {
        Field<BooleanGraphType>("Success").Description("Succesmessage");
        Field<StringGraphType>("Message").Description("The Responsemessage");
        Field<StringGraphType>("Token").Description("The token");
    }

}