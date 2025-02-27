using todListBackend.Graphql.Types;
using todListBackend.Services;


public class Query
{
    
    private readonly TodoService _todoService;

    public Query(TodoService todoService)
    {
        _todoService = todoService;
    }
    
    public string Hello => "Hello, GraphQL!";

    /*
    public async Task<List<TodolistType>> GetAllTodolists(string currentUserId)
    {
        return await _todoService.GetAllTodolists(currentUserId);
    }
    */
}