using Basecamp1.Data;
using Microsoft.AspNetCore.Mvc;

namespace Basecamp1.Controllers;

public class DashboardController : Controller
{
    private readonly AppDbContext _context;

    public DashboardController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        ViewBag.TotalProjects = _context.Projects.Count();

        ViewBag.TotalUsers = _context.Users.Count();

        return View();
    }
}