using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace BaseCamp1MVC.Controllers;

public class AccountController : Controller
{
    private readonly AppDbContext _context;

    public AccountController(AppDbContext context)
    {
        _context = context;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(User user)
    {
        _context.Users.Add(user);
        _context.SaveChanges();

        return RedirectToAction("Login");
    }

    public IActionResult Login()
    {
        if (HttpContext.Session.GetInt32("UserId") != null)
            return RedirectToAction("Index", "Dashboard");

        return View();
    }

    [HttpPost]
    public IActionResult Login(string email, string password)
    {
        var user = _context.Users
            .FirstOrDefault(x => x.Email == email && x.Password == password);

        if (user == null)
        {
            ViewBag.Error = "Email və ya şifrə səhvdir";
            return View();
        }

        
        HttpContext.Session.SetInt32("UserId", user.Id);
        HttpContext.Session.SetString("UserName", user.FullName);
        HttpContext.Session.SetString("IsAdmin", user.IsAdmin.ToString().ToLower());
        return RedirectToAction("Index", "Projects");
    }

    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction("Login");
    }
}