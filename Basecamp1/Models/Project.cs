namespace Basecamp1.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}