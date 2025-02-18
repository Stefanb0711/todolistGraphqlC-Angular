namespace todListBackend.Graphql.Types;

public class ResponseDeletetodolistType
{
    public string? Message { get; set; }
    public bool? Success { get; set; }
    public List<TodolistType>? AllTodolists { get; set; }
}