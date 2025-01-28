using GraphQL;
using GraphQL.Types;
using todListBackend.Graphql.Types;

namespace todListBackend.Graphql.Mutations;

public class AuthMutation : ObjectGraphType
{
    
    private readonly AuthService _authService;
    
    public AuthMutation(AuthService authService)
    {
        _authService = authService;
        
        Field<RegisterResponseType>("register",
            arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "username" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "email" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "passwordConfirm" }
            ),
            resolve: context =>
            {

                var registerData = new RegisterModel
                {
                    Username = context.GetArgument<string>("username"),
                    Email = context.GetArgument<string>("email"),
                    Password = context.GetArgument<string>("password"),
                    PasswordConfirm = context.GetArgument<string>("passwordConfirm")
                };

                return _authService.Register(registerData);
            });

        Field<LoginResponseType>("login", arguments: new QueryArguments(
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "usernameOrEmail" },
                new QueryArgument<NonNullGraphType<StringGraphType>> { Name = "password" }
            ),
            resolve: context =>
            {
                var loginData = new LoginModel
                {
                    UsernameOrEmail = context.GetArgument<string>("usernameOrEmail"),
                    Password = context.GetArgument<string>("password")
                };
                
                return _authService.Login(loginData);
                
            });

    }
}