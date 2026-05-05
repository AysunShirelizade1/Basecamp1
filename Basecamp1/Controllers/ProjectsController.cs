using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basecamp1.Controllers;

public class ProjectsController : Controller
{
    private readonly AppDbContext _context;

    public ProjectsController(AppDbContext context)
    {
        _context = context;
    }


    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var projects = _context.Projects
            .Where(x => x.UserId == userId.Value)
            .ToList();

        return View(projects);
    }

    public IActionResult Create()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Create(Project project)
    {
        var user = GetCurrentUser();

        if (user == null)
            return RedirectToAction("Login", "Account");

        project.UserId = user.Id;

        _context.Projects.Add(project);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Details(int id)
    {
        var user = GetCurrentUser();
        if (user == null)
            return RedirectToAction("Login", "Account");

        var project = _context.Projects.FirstOrDefault(x => x.Id == id);

        if (project == null) return NotFound();

        if (!user.IsAdmin && project.UserId != user.Id)
            return Content("Access Denied");

        return View(project);
    }

    public IActionResult Edit(int id)
    {
        var user = GetCurrentUser();
        if (user == null)
            return RedirectToAction("Login", "Account");

        var project = _context.Projects.FirstOrDefault(x => x.Id == id);

        if (project == null) return NotFound();

        if (!user.IsAdmin && project.UserId != user.Id)
            return Content("Access Denied");

        return View(project);
    }

    [HttpPost]
    public IActionResult Edit(Project project)
    {
        var user = GetCurrentUser();
        if (user == null)
            return RedirectToAction("Login", "Account");

        var existing = _context.Projects.AsNoTracking()
            .FirstOrDefault(x => x.Id == project.Id);

        if (existing == null) return NotFound();

        if (!user.IsAdmin && existing.UserId != user.Id)
            return Content("Access Denied");

        project.UserId = existing.UserId;

        _context.Projects.Update(project);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }

    public IActionResult Delete(int id)
    {
        var user = GetCurrentUser();
        if (user == null)
            return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(id);

        if (project == null) return NotFound();

        if (!user.IsAdmin && project.UserId != user.Id)
            return Content("Access Denied");

        _context.Projects.Remove(project);
        _context.SaveChanges();

        return RedirectToAction("Index");
    }
    private User? GetCurrentUser()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null) return null;

        return _context.Users.Find(userId);
    }
}