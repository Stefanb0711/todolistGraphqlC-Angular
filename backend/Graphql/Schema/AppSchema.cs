namespace todListBackend.Graphql.Schema;

public class AppSchema : Schema
{
    public AppSchema(IServiceProvider serviceProvider)
        : base(serviceProvider)
    {
        Query = serviceProvider.GetRequiredService<AppQuery>();
        Mutation = serviceProvider.GetRequiredService<AppMutation>();
    }   
}