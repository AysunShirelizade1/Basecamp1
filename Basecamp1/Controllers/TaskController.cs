using Microsoft.AspNetCore.Mvc;
using Basecamp1.Data;
using Basecamp1.Models;
namespace Basecamp1.Controllers;

public class TaskController : Controller
{
    private readonly AppDbContext _context;
    public TaskController(AppDbContext context)
    {
        _context = context;
    }
    [HttpPost]
    public IActionResult Create(TaskItem task)
    {
        _context.TaskItems.Add(task);
        _context.SaveChanges();
        return RedirectToAction("Details", "Project", new { id = task.ProjectId });
    }
    public IActionResult Complete(int id)
    {
        var task = _context.TaskItems.Find(id);
        if(task == null)
        {
            return NotFound();
        }
        task.IsCompleted = true;
        _context.SaveChanges();
        return RedirectToAction("Details", "Project", new { id = task.ProjectId });

    }
    public IActionResult Delete (int id)
    {
        var task = _context.TaskItems.Find(id);
        if (task == null)
        {
            return NotFound();
        }
        int projectId = task.ProjectId;
        _context.TaskItems.Remove(task);
        _context.SaveChanges();
        return RedirectToAction("Details", "Project", new { id = projectId });
    }

}
