namespace todListBackend.Graphql.Types;

public class DeleteTodoInput
{
    public string todoId { get; set; }
    public string todolistId { get; set; }
}