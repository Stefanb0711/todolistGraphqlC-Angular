using todListBackend.Graphql.Mutations;

namespace todListBackend.Graphql.Schema;

public class AppSchema : GraphQL.Types.Schema
{
    public AppSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<Query>();
        Mutation = serviceProvider.GetRequiredService<AuthMutation>();
    }   
}