using Basecamp1.Data;
using Basecamp1.Models;
using Microsoft.AspNetCore.Mvc;

namespace Basecamp1.Controllers;

public class AttachmentsController : Controller
{
    private readonly AppDbContext _context;
    private readonly IWebHostEnvironment _env;

    public AttachmentsController(AppDbContext context, IWebHostEnvironment env)
    {
        _context = context;
        _env = env;
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

        // Faylı saxla
        var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads");
        Directory.CreateDirectory(uploadsFolder);

        var uniqueName = Guid.NewGuid().ToString() + ext;
        var filePath = Path.Combine(uploadsFolder, uniqueName);

        using (var stream = new FileStream(filePath, FileMode.Create))
        {
            await file.CopyToAsync(stream);
        }

        var attachment = new Attachment
        {
            FileName = file.FileName,
            FilePath = "/uploads/" + uniqueName,
            Format = ext.TrimStart('.'),
            ProjectId = projectId,
            UserId = userId.Value
        };

        _context.Attachments.Add(attachment);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }

    public IActionResult Delete(int id)
    {
        var userId = HttpContext.Session.GetInt32("UserId");
        if (userId == null) return RedirectToAction("Login", "Account");

        var attachment = _context.Attachments.Find(id);
        if (attachment == null) return NotFound();

        // Faylı diskdən sil
        var fullPath = Path.Combine(_env.WebRootPath, attachment.FilePath.TrimStart('/'));
        if (System.IO.File.Exists(fullPath))
            System.IO.File.Delete(fullPath);

        int projectId = attachment.ProjectId;
        _context.Attachments.Remove(attachment);
        _context.SaveChanges();

        return RedirectToAction("Details", "Projects", new { id = projectId });
    }
}