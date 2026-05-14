namespace Basecamp1.Models;

public class Project : BaseEntity
{
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
    public int? AdminId { get; set; }
    public User? Admin { get; set; }

    public List<Attachment> Attachments { get; set; } = new();
    public List<ForumThread> Threads { get; set; } = new();
    public List<TaskItem> Tasks { get; set; } = new List<TaskItem>();
    public List<ProjectMember> Members { get; set; } = new();

}