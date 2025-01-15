
using Marten;
using MartenStuff.Tagging.EventStore;
using Wolverine.Marten;

namespace MartenStuff.Tagging.Model;


[AggregateHandler(AggregateType = typeof(Tagging))]
[MartenStore(typeof(ITaggingStore))]
public static class CommandHandlers
{
    public static IEnumerable<object> Handle(UpdateTagging command, Tagging? tagging, IDocumentSession session)
    {
        Console.WriteLine($"Updating Tagging: {session.DocumentStore.Options.DatabaseSchemaName}");
        Console.WriteLine($"Updating Tagging {tagging?.Name}");
        var e = new TaggingUpdated(command.Name);
        yield return e;
    }
}