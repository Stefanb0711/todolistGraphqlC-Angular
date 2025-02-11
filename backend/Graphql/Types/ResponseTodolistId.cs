namespace todListBackend.Graphql.Types;

public class ResponseTodolistId
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public string? TodolistId { get; set; }
}