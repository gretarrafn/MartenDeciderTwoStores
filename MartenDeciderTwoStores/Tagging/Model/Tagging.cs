using Marten.Schema;

namespace MartenStuff.Tagging.Model;

public class Tagging
{
    public Guid Id { get; set; }
    public string? Name { get; set; }

    public void Apply(TaggingCreated created)
    {
        Name = created.Name;
    }
    
    public void Apply(TaggingUpdated updated)
    {
        Name = updated.Name;
    }
}