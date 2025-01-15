using Marten;
using MartenStuff.Tagging.EventStore;
using MartenStuff.Tagging.Model;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Wolverine;

namespace MartenStuff.Tagging.Controllers;

[ApiController]
public class TaggingController
{
    [HttpPost("/tagging")]
    public async Task<ActionResult<Guid>> Post(
        [FromBody] string command,
        [FromServices] ITaggingStore store
    )
    {
        await using var session = store.LightweightSession();
        var taggingCreated = new TaggingCreated(command);  // Replace 'null' with a valid value as needed
        var res = session.Events.StartStream<Tagging.Model.Tagging>(taggingCreated);
        await session.SaveChangesAsync();
        return res.Id;
    }
    
    [HttpPost("/tagging/{id}")]
    public async Task<IActionResult> Update(
        Guid id,
        [FromBody] UpdateTagging command,
        [FromServices] IMessageBus bus
    )
    {
        await bus.InvokeAsync(command with { Id = id });
        return new OkResult();
    }


    [HttpGet("/tagging/{id}")]
    public async Task<ActionResult<Tagging.Model.Tagging?>> Get(
        Guid id,
        [FromServices] ITaggingStore store
    )
    {
        await using var session = store.LightweightSession();
        var tag = await session.LoadAsync<Tagging.Model.Tagging>(id);
        Console.WriteLine(id);
        Console.WriteLine(tag);
        return tag;
    }
}
