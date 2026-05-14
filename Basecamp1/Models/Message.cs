namespace Basecamp1.Models;

public class Message : BaseEntity
{
    public string Content { get; set; } = string.Empty;

    public int ThreadId { get; set; }
    public ForumThread? Thread { get; set; }  

    public int UserId { get; set; }
    public User? User { get; set; }
}