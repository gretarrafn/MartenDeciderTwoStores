using Marten.Events.Aggregation;

namespace MartenStuff.Tagging.Model;

public class TaggingProjection: SingleStreamProjection<Tagging>
{
    public Tagging Create(TaggingCreated created)
    {
        return new Tagging { Name = created.Name };
    }
    
    public void Apply(TaggingUpdated updated, Tagging tagging) => tagging.Name = updated.Name;
}