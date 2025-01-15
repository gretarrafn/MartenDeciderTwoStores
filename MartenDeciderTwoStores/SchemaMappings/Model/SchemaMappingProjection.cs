using Marten.Events.Aggregation;

namespace MartenStuff.SchemaMappings.Model;

public class SchemaMappingProjection: SingleStreamProjection<SchemaMapping>
{
    public SchemaMapping Create(SchemaMappingCreated created)
    {
        return new SchemaMapping { Name = created.Name };
    }
    
    public void Apply(SchemaMappingUpdated updated, SchemaMapping mapping) => mapping.Name = updated.Name;
    
}
