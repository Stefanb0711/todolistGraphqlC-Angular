namespace todListBackend.Graphql.Types;

public class DeleteTodolist
{
    public string TodolistId { get; set; }
    
    public string UserId { get; set; }
}