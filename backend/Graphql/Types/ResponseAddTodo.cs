namespace todListBackend.Graphql.Types;

public class ResponseAddTodo
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public List<TodoType>? Todos { get; set; }
}