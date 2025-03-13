using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using NetProjektNews.Data;
using NetProjektNews.Models;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Processing;

namespace NetProjektNews.Controllers
{
    public class ArticleController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly IWebHostEnvironment _hostEnvironment; // Läs ut sökvägar till filsystem

        private readonly string wwwRootPath; // Sökväg till wwwroot

        public ArticleController(ApplicationDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _context = context;
            _hostEnvironment = hostEnvironment;
            wwwRootPath = hostEnvironment.WebRootPath;
        }

        // GET: Article
        public async Task<IActionResult> Index()
        {
            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return NotFound();
            }

            // Visa nyheter med nyaste överst
            var applicationDbContext = _context.Articles
            .Include(a => a.Category)
            .OrderByDescending(a => a.CreatedAt);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: Article/Details/5
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return NotFound();
            }


            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // GET: Article/Create
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName");
            return View();
        }

        // POST: Article/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Create([Bind("Id,Title,Content,CreatedAt,ImageFile,CategoryId")] Article article)
        {
            if (ModelState.IsValid)
            {
                // Kontrollera om det finns bild 
                if (article.ImageFile != null)
                {
                    // Generera unikt filnamn 
                    string fileName = Path.GetFileNameWithoutExtension(article.ImageFile.FileName);
                    string extension = Path.GetExtension(article.ImageFile.FileName); //Filändelse

                    article.ImageName = fileName = fileName.Replace(" ", String.Empty) + DateTime.Now.ToString("yymmssfff") + extension; // Ta bort mellanslag om det finns och lägg till datum + filändelse 

                    string path = Path.Combine(wwwRootPath + "/images", fileName);

                    // Spara i filsystem 
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await article.ImageFile.CopyToAsync(fileStream);
                    }

                    // Skapa minityr-bild 
                    CreateImageFiles(fileName);

                }
                else
                {
                    article.ImageName = "placeholder.jpg"; // Om det inte finns någon bild, använd placeholder-bild 
                }

                _context.Add(article);

                // Lägg till inloggad användare till createdBy
                article.CreatedBy = User.Identity?.Name ?? "Unknown";

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", article.CategoryId);
            return View(article);
        }

        // GET: Article/Edit/5
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", article.CategoryId);
            return View(article);
        }

        // POST: Article/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Edit(int id, [Bind("Id,Title,Content,CreatedAt,ImageName,CategoryId")] Article article)
        {
            if (id != article.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(article);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ArticleExists(article.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Categories, "Id", "CategoryName", article.CategoryId);
            return View(article);
        }

        // GET: Article/Delete/5
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return NotFound();
            }

            var article = await _context.Articles
                .Include(a => a.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (article == null)
            {
                return NotFound();
            }

            return View(article);
        }

        // POST: Article/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return NotFound();
            }

            var article = await _context.Articles.FindAsync(id);
            if (article != null)
            {
                _context.Articles.Remove(article);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ArticleExists(int id)
        {
            // Kontrollera om context är null 
            if (_context.Articles == null)
            {

                return false;
            }
            return _context.Articles.Any(e => e.Id == id);
        }

        private void CreateImageFiles(string fileName)
        {
            string imagePath = wwwRootPath + "/images/";

            // Skapa miniatyr 
            using var image = Image.Load(imagePath + fileName);
            // Gör bilden hälften så stor som original 
            image.Mutate(x => x.Resize(image.Width / 2, image.Height / 2));
            // Spara bild med "thumb" i namnet 
            image.Save(imagePath + "thumb_" + fileName);
        }
    }
}
