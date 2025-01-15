using Marten;
using MartenDeciderTwoStores.SchemaMappings.Model;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace MartenDeciderTwoStores.SchemaMappings.Controllers;

[ApiController]
public class PostsController
{
    [HttpPost("/custom_data_schema_mapping")]
    public async Task<ActionResult<Guid>> Post(
        [FromBody] string command,
        [FromServices] IDocumentSession session
    )
    {
        var schemaMappingCreated = new SchemaMappingCreated(command);
        var stream = session.Events.StartStream<SchemaMapping>(schemaMappingCreated);
        await session.SaveChangesAsync();
        return stream.Id;
    }
    
    [HttpPost("/custom_data_schema_mapping/{id}")]
    public async Task<ActionResult> Post(
        Guid id,
        UpdateSchemaMapping command,
        [FromServices] IMessageBus bus
    )
    {
        await bus.InvokeAsync(command with {Id = id});
        return new OkResult();
    }
    
    [HttpGet("/custom_data_schema_mapping/{id}")]
    public async Task<SchemaMapping?> Post(
        Guid id,
        [FromServices] IDocumentSession session
    )
    {
        return await session.LoadAsync<SchemaMapping>(id);
    }
}
