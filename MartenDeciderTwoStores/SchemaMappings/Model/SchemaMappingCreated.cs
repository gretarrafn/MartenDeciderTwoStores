namespace MartenStuff.SchemaMappings.Model;

public record SchemaMappingCreated(string? Name)
{
    public string? Name { get; set; } = Name;
}

public record SchemaMappingUpdated(string? Name)
{
    public string? Name { get; set; } = Name;
}

public record SchemaMappingDeleted();