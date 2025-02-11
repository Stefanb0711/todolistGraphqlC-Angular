namespace todListBackend.Graphql.Types;

public class AddTodolistInput
{
    public string? Id { get; set; }
    
    public string Name { get; set; }

    public string UserId { get; set; }

    public long Date { get; set; }
}