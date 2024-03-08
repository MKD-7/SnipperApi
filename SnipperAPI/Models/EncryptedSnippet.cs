
public record EncryptedSnippet(Guid Id, string Title, byte[] Content, DateTime CreatedAt)
{
    public string Lang { get; set; }
}
