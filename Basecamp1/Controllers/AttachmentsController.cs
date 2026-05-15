using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basecamp1.Controllers;

public class AttachmentsController : Controller
{
    private readonly AppDbContext _context;

    public AttachmentsController(AppDbContext context)
    {
        _context = context;
    }

    [HttpPost]
    public async Task<IActionResult> Create(int projectId, IFormFile file)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var project = _context.Projects.Find(projectId);
        if (project == null) return NotFound();

        if (file == null || file.Length == 0)
        {
            TempData["Error"] = "Fayl seçilməyib.";
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        var allowedFormats = new[] { ".png", ".jpg", ".jpeg", ".pdf", ".txt" };
        var ext = Path.GetExtension(file.FileName).ToLower();

        if (!allowedFormats.Contains(ext))
        {
            TempData["Error"] = "Yalnız png, jpg, pdf, txt formatları qəbul edilir.";
            return RedirectToAction("Details", "Projects", new { id = projectId });
        }

        using var ms = new MemoryStream();
        await file.CopyToAsync(ms);

        var attachment = new Attachment
        {
            FileName = file.FileName,
            Format = ext.TrimStart('.'),
            FileData = ms.ToArray(),
            ContentType = file.ContentType,
            ProjectId = projectId,
            UserId = userId.Value
        };

        _context.Attachments.Add(attachment);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    public IActionResult Download(int id)
    {
        var attachment = _context.Attachments.Find(id);
        if (attachment == null) return NotFound();

        return File(attachment.FileData, attachment.ContentType, attachment.FileName);
    }

    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var attachment = _context.Attachments.Find(id);
        if (attachment == null) return NotFound();

        int projectId = attachment.ProjectId;
        _context.Attachments.Remove(attachment);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}