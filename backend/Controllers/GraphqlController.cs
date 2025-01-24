using GraphQL;
using GraphQL.Types;
using Microsoft.AspNetCore.Mvc;
using todListBackend.Graphql.Types;

namespace todListBackend.Controllers;

[Route("graphql")]
//[ServiceFilter(typeof(TokenValidationFilter))]

public class GraphqlController : Controller
{
    public GraphqlController(ISchema schema, IDocumentExecuter documentExecuter)
    {
        /*
        _schema = schema;
        _documentExecuter = documentExecuter;*/
    }
    
    [HttpPost] 
    public async Task<IActionResult> Post([FromBody] GraphQLQuery query)
    {
        if (query == null)
        {
            return BadRequest("Fehlende GraphQL-Abfrage.");
        }

        /*
        var executionOptions = new ExecutionOptions
        {
            Schema = _schema,
            Query = query.Query,
            Inputs = query.Variables?.ToInputs()
        };
        
        var result = await _documentExecuter.ExecuteAsync(executionOptions);

        // Fehlerbehandlung und Antwort
        if (result.Errors?.Count > 0)
        {
            return BadRequest(result.Errors);
        }

        return Ok(result.Data);
        */
    }
    
}