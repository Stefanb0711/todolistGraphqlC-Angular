using GraphQL.Types;
using todoList.Models;


public class UserType : ObjectGraphType<User>
{

    public UserType()
    {
        Field(x => x.Id).Description("The id of the user.");
        Field(x => x.Username).Description("The name of the user");
        Field(x => x.Email).Description("The email of the user");
        Field(x => x.Password).Description("The password of the user");
        Field(x => x.Todos).Description("The todlists of the user");

    }

   
   
}