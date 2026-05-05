using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basecamp1.Controllers;

public class AdminController : Controller
{
    private readonly AppDbContext _context;

    public AdminController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Users()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var currentUser = _context.Users.Find(userId);

        if (currentUser == null || !currentUser.IsAdmin)
            return Content("Access Denied");
        var users = _context.Users.ToList();
        return View(users);
    }

    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var currentUser = _context.Users.Find(userId);

        if (currentUser == null || !currentUser.IsAdmin)
            return Content("Access Denied");
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        _context.Users.Remove(user);
        _context.SaveChanges();

        return RedirectToAction("Users");
    }

    public IActionResult SetAdmin(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var currentUser = _context.Users.Find(userId);

        if (currentUser == null || !currentUser.IsAdmin)
            return Content("Access Denied");
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.IsAdmin = true;
        _context.SaveChanges();

        return RedirectToAction("Users");
    }

    public IActionResult RemoveAdmin(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var currentUser = _context.Users.Find(userId);

        if (currentUser == null || !currentUser.IsAdmin)
            return Content("Access Denied");
        var user = _context.Users.Find(id);
        if (user == null) return NotFound();

        user.IsAdmin = false;
        _context.SaveChanges();

        return RedirectToAction("Users");
    }
}