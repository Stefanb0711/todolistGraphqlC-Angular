namespace todListBackend.Graphql.Types;

public class ResponseAddTodolistType
{
    public string? Message { get; set; }
    public bool? Success { get; set; }
    public List<TodolistType>? AllTodolists { get; set; }
}