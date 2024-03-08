
public record Snippet(Guid Id, string Title, string Content, DateTime CreatedAt)
{
    public string Lang { get; set; }
}
