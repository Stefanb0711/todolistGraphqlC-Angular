namespace todListBackend.Graphql.Types;

public class TodoType
{
    public string? Id {get;set;}
    
    public string Content { get; set;}
    
    public long? Date { get; set;}
    
    public string TodolistId { get; set; }
}