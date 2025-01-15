namespace MartenDeciderTwoStores.SchemaMappings.Model;

public record CreateSchemaMapping(string? Name);
public record UpdateSchemaMapping(Guid Id, string Name);
public record DeleteSchemaMapping();