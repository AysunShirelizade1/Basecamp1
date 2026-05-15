using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basecamp1.Controllers;

public class ProjectMembersController : Controller
{
    private readonly AppDbContext _context;

    public ProjectMembersController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Add(int projectId, int userId)
    {
        var currentUserId = HttpContext.Session.GetInt32("UserId");
        if (currentUserId == null) return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();

        if (project.AdminId != currentUserId.Value)
            return Content("Access Denied - Yalnız admin member əlavə edə bilər.");

        // Artıq member deyilsə əlavə et
        var exists = _context.ProjectMembers
            .Any(pm => pm.ProjectId == projectId && pm.UserId == userId);

        if (!exists)
        {
            _context.ProjectMembers.Add(new ProjectMember
            {
                ProjectId = projectId,
                UserId = userId
            });
            _context.SaveChanges();
        }

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    public IActionResult Remove(int projectId, int userId)
    {
        var currentUserId = HttpContext.Session.GetInt32("UserId");
        if (currentUserId == null) return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();

        if (project.AdminId != currentUserId.Value)
            return Content("Access Denied");

        var member = _context.ProjectMembers
            .FirstOrDefault(pm => pm.ProjectId == projectId && pm.UserId == userId);

        if (member != null)
        {
            _context.ProjectMembers.Remove(member);
            _context.SaveChanges();
        }

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}