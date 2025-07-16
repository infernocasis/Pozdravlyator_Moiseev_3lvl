using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Pozdravlyator.Data;
using Pozdravlyator.Models;

namespace Pozdravlyator.Controllers
{
    public class BirthdaysController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _environment;

        public BirthdaysController(ApplicationDbContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Birthdays
        public async Task<IActionResult> Index()
        {
            var birthdays = await _context.Birthdays.OrderBy(b => b.Date.Month).ThenBy(b => b.Date.Day).ToListAsync();
            return View(birthdays);
        }

        // GET: Birthdays/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null) return NotFound();
            var birthday = await _context.Birthdays.FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null) return NotFound();
            return View(birthday);
        }

        // GET: Birthdays/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Birthdays/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Birthday birthday, IFormFile? photo)
        {
            if (ModelState.IsValid)
            {
                if (photo != null && photo.Length > 0)
                {
                    var uploads = Path.Combine(_environment.WebRootPath, "photos");
                    Directory.CreateDirectory(uploads);
                    var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                    var filePath = Path.Combine(uploads, fileName);
                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        await photo.CopyToAsync(stream);
                    }
                    birthday.PhotoPath = "/photos/" + fileName;
                }
                _context.Add(birthday);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(birthday);
        }

        // GET: Birthdays/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();
            var birthday = await _context.Birthdays.FindAsync(id);
            if (birthday == null) return NotFound();
            return View(birthday);
        }

        // POST: Birthdays/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Birthday birthday, IFormFile? photo)
        {
            if (id != birthday.Id) return NotFound();
            if (ModelState.IsValid)
            {
                try
                {
                    if (photo != null && photo.Length > 0)
                    {
                        var uploads = Path.Combine(_environment.WebRootPath, "photos");
                        Directory.CreateDirectory(uploads);
                        var fileName = Guid.NewGuid() + Path.GetExtension(photo.FileName);
                        var filePath = Path.Combine(uploads, fileName);
                        using (var stream = new FileStream(filePath, FileMode.Create))
                        {
                            await photo.CopyToAsync(stream);
                        }
                        birthday.PhotoPath = "/photos/" + fileName;
                    }
                    _context.Update(birthday);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BirthdayExists(birthday.Id)) return NotFound();
                    else throw;
                }
                return RedirectToAction(nameof(Index));
            }
            return View(birthday);
        }

        // GET: Birthdays/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();
            var birthday = await _context.Birthdays.FirstOrDefaultAsync(m => m.Id == id);
            if (birthday == null) return NotFound();
            return View(birthday);
        }

        // POST: Birthdays/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var birthday = await _context.Birthdays.FindAsync(id);
            if (birthday != null)
            {
                if (!string.IsNullOrEmpty(birthday.PhotoPath))
                {
                    var filePath = Path.Combine(_environment.WebRootPath, birthday.PhotoPath.TrimStart('/').Replace('/', Path.DirectorySeparatorChar));
                    if (System.IO.File.Exists(filePath))
                        System.IO.File.Delete(filePath);
                }
                _context.Birthdays.Remove(birthday);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool BirthdayExists(int id)
        {
            return _context.Birthdays.Any(e => e.Id == id);
        }
    }
} 