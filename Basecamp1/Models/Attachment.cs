namespace Basecamp1.Models;

public class Attachment : BaseEntity
{
    public string FileName { get; set; } = string.Empty;
    public string Format { get; set; } = string.Empty;
    public byte[] FileData { get; set; } = Array.Empty<byte>();
    public string ContentType { get; set; } = string.Empty;

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public int UserId { get; set; }
    public User? User = null;
}