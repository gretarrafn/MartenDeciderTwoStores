namespace MartenStuff.Tagging.Model;

public record UpdateTagging
{
    public Guid Id { get; set; }
    public string Name { get; set; }
}