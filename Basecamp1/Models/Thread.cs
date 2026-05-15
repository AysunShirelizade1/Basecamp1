namespace Basecamp1.Models;

public class ForumThread : BaseEntity  
{
    public string Title { get; set; } = string.Empty;

    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }

    public List<Message> Messages { get; set; } = new();
}