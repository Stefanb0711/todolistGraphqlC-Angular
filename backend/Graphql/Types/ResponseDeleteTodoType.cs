namespace todListBackend.Graphql.Types;

public class ResponseDeleteTodoType
{
    public bool? Success { get; set; }
    public string? Message { get; set; }
    public string? TodolistId { get; set; }
    public List<TodoType>? Todos { get; set; }
}