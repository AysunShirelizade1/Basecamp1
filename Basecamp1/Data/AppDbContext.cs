namespace Basecamp1.Data;

using Microsoft.EntityFrameworkCore;
using Basecamp1.Models;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Project> Projects { get; set; } = null!;
    public DbSet<TaskItem> TaskItems { get; set; } = null!;
}