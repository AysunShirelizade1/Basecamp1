using Basecamp1.Data;
using Microsoft.AspNetCore.Mvc;

namespace Basecamp1.Controllers;

public class ProfileController : Controller
{
    private readonly AppDbContext _context;

    public ProfileController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Index()
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = _context.Users.Find(userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        return View(user);
    }

    [HttpPost]
    public IActionResult UpdateProfile(string fullName, string email)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = _context.Users.Find(userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        user.FullName = fullName;
        user.Email = email;

        _context.SaveChanges();

        HttpContext.Session.SetString("UserName", user.FullName);

        TempData["Success"] = "Profile updated successfully.";

        return RedirectToAction("Index");
    }

    [HttpPost]
    public IActionResult ChangePassword(string currentPassword, string newPassword, string confirmPassword)
    {
        var userId = HttpContext.Session.GetInt32("UserId");

        if (userId == null)
            return RedirectToAction("Login", "Account");

        var user = _context.Users.Find(userId.Value);

        if (user == null)
            return RedirectToAction("Login", "Account");

        if (user.Password != currentPassword)
        {
            TempData["Error"] = "Current password is incorrect.";
            return RedirectToAction("Index");
        }

        if (newPassword != confirmPassword)
        {
            TempData["Error"] = "New passwords do not match.";
            return RedirectToAction("Index");
        }

        user.Password = newPassword;

        _context.SaveChanges();

        TempData["Success"] = "Password changed successfully.";

        return RedirectToAction("Index");
    }
}