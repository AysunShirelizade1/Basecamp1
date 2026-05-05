namespace Basecamp1.Models;

public class User : BaseEntity
{
    public string FullName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;

    public bool IsAdmin { get; set; } = false;

    public List<Project> Projects { get; set; } = new();
}
