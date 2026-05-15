using Microsoft.AspNetCore.Mvc;
using Basecamp1.Data;
using Basecamp1.Models;

namespace Basecamp1.Controllers;

public class TasksController : Controller
{
    private readonly AppDbContext _context;

    public TasksController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        task.UserId = userId.Value;

        _context.TaskItem.Add(task);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
    }

    public IActionResult Complete(int id)
    {
        var task = _context.TaskItem.Find(id);
        if (task == null) return NotFound();

        task.IsCompleted = true;
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = task.ProjectId });
    }

    public IActionResult Delete(int id)
    {
        var task = _context.TaskItem.Find(id);
        if (task == null) return NotFound();

        int projectId = task.ProjectId;

        _context.TaskItem.Remove(task);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}