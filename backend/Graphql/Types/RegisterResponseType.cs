using GraphQL.Types;

namespace todListBackend.Graphql.Types;

public class RegisterResponseType: ObjectGraphType
{
    public RegisterResponseType()
    {
        Field<BooleanGraphType>("Success").Description("Succesmessage");
        Field<StringGraphType>("Message").Description("The Responsemessage");
    }
    
}