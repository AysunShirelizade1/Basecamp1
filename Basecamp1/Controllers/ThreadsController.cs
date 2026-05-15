using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basecamp1.Controllers;

public class ThreadsController : Controller
{
    private readonly AppDbContext _context;

    public ThreadsController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Create(int projectId)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();

        // Yalnız project admin yarada bilər
        if (project.AdminId != userId.Value)
            return Content("Access Denied - Yalnız project admin thread yarada bilər.");

        ViewBag.ProjectId = projectId;
        return View();
    }

    [HttpPost]
    public IActionResult Create(ForumThread thread)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(thread.ProjectId);
        if (project == null) return NotFound();

        if (project.AdminId != userId.Value)
            return Content("Access Denied - Yalnız project admin thread yarada bilər.");

        thread.UserId = userId.Value;
        _context.Threads.Add(thread);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = thread.ProjectId });
    }

    public async Task<IActionResult> Show(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var thread = await _context.Threads
            .Include(t => t.Project)
            .Include(t => t.User)
            .Include(t => t.Messages)
                .ThenInclude(m => m.User)
            .FirstOrDefaultAsync(t => t.Id == id);

        if (thread == null) return NotFound();

        return View(thread);
    }

    public IActionResult Edit(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var thread = _context.Threads.Include(t => t.Project).FirstOrDefault(t => t.Id == id);
        if (thread == null) return NotFound();

        if (thread.Project!.AdminId != userId.Value)
            return Content("Access Denied");

        return View(thread);
    }

    [HttpPost]
    public IActionResult Edit(ForumThread thread)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var existing = _context.Threads.Include(t => t.Project)
            .FirstOrDefault(t => t.Id == thread.Id);
        if (existing == null) return NotFound();

        if (existing.Project!.AdminId != userId.Value)
            return Content("Access Denied");

        existing.Title = thread.Title;
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = existing.ProjectId });
    }

    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var thread = _context.Threads.Include(t => t.Project)
            .FirstOrDefault(t => t.Id == id);
        if (thread == null) return NotFound();

        if (thread.Project!.AdminId != userId.Value)
            return Content("Access Denied");

        int projectId = thread.ProjectId;
        _context.Threads.Remove(thread);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}