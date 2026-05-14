namespace Basecamp1.Models;

public class ProjectMember : BaseEntity
{
    public int ProjectId { get; set; }
    public Project? Project { get; set; }

    public int UserId { get; set; }
    public User? User { get; set; }
}