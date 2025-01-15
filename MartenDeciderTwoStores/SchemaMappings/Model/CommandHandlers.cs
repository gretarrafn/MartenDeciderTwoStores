using Marten;
using Wolverine.Marten;

namespace MartenStuff.SchemaMappings.Model;

[AggregateHandler(AggregateType = typeof(SchemaMapping))]
public static class CommandHandlers
{
    [AggregateHandler(AggregateType = typeof(SchemaMapping))]
    public static IEnumerable<object> Handle(UpdateSchemaMapping command, SchemaMapping schemaMapping, IDocumentSession session)
    {
        Console.WriteLine($"Updating SchemaMapping: {session.DocumentStore.Options.DatabaseSchemaName}");
        Console.WriteLine($"Updating SchemaMapping {schemaMapping}");
        var e = new SchemaMappingUpdated(command.Name);
        yield return e;
    }
}