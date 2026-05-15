using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Basecamp1.Controllers;

public class MessagesController : Controller
{
    private readonly AppDbContext _context;

    public MessagesController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public IActionResult Create(Message message)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        message.UserId = userId.Value;
        _context.Messages.Add(message);
        _context.SaveChanges();

        return RedirectToAction("Show", "Threads", new { id = message.ThreadId });
    }

    public IActionResult Edit(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var message = _context.Messages.Find(id);
        if (message == null) return NotFound();

        if (message.UserId != userId.Value)
            return Content("Access Denied");

        return View(message);
    }

    [HttpPost]
    public IActionResult Edit(Message message)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var existing = _context.Messages.Find(message.Id);
        if (existing == null) return NotFound();

        if (existing.UserId != userId.Value)
            return Content("Access Denied");

        existing.Content = message.Content;
        _context.SaveChanges();

        return RedirectToAction("Show", "Threads", new { id = existing.ThreadId });
    }

    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var message = _context.Messages.Find(id);
        if (message == null) return NotFound();

        if (message.UserId != userId.Value)
            return Content("Access Denied");

        int threadId = message.ThreadId;
        _context.Messages.Remove(message);
        _context.SaveChanges();

        return RedirectToAction("Show", "Threads", new { id = threadId });
    }
}