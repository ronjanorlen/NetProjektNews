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


namespace NetProjektNews.Controllers
{
    [Authorize(Roles = "Admin, editor")] // Skyddad - måste vara inloggad för åtkomst
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _context;

        public CategoryController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Category
        [Authorize(Roles = "Admin, Editor")] // Admin och editor har tillgång
        public async Task<IActionResult> Index()
        {
            // Kontrollera om context är null 
            if (_context.Categories == null)
            {

                return NotFound();
            }

            return View(await _context.Categories.ToListAsync());
        }

        // GET: Category/Details/5
        [Authorize(Roles = "Admin, Editor")] // // Admin och editor har tillgång
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Categories == null)
            {

                return NotFound();
            }

            var category = await _context.Categories
            .Include(c => c.Articles) // Hämta tillhörande nyheter 
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // GET: Category/Create
        [Authorize(Roles = "Admin, Editor")] // Bara admin och editor har tillgång
        public IActionResult Create()
        {
            return View();
        }

        // POST: Category/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")] // Bara admin och editor har tillgång
        public async Task<IActionResult> Create([Bind("Id,CategoryName")] Category category)
        {
            // Kontrollera om kategori redan finns 
            if (_context.Categories.Any(c => c.CategoryName == category.CategoryName))
            {
                // Isåfall skriv ut felmeddelande 
                ModelState.AddModelError("CategoryName", "Det finns redan en kategori med detta namn");
                return View(category);
            }
            // Annars lägg till ny kategori 
            if (ModelState.IsValid)
            {
                _context.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        // GET: Category/Edit/5
        [Authorize(Roles = "Admin, Editor")] // Bara admin och editor har tillgång
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Categories == null)
            {

                return NotFound();
            }

            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound();
            }
            return View(category);
        }

        // POST: Category/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin, Editor")] // Bara admin och editor har tillgång
        public async Task<IActionResult> Edit(int id, [Bind("Id,CategoryName")] Category category)
        {
            if (id != category.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(category);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!CategoryExists(category.Id))
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
            return View(category);
        }

        // GET: Category/Delete/5
        [Authorize(Roles = "Admin")] // Bara admin har tillgång
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Kontrollera om context är null 
            if (_context.Categories == null)
            {

                return NotFound();
            }

            var category = await _context.Categories
                .Include(c => c.Articles) // Hämta tillhörande nyheter
                .FirstOrDefaultAsync(m => m.Id == id);
            if (category == null)
            {
                return NotFound();
            }

            return View(category);
        }

        // POST: Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "Admin")] // Bara admin har tillgång
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var category = await _context.Categories
                .Include(c => c.Articles) // Hämta tillhörande nyheter
                .FirstOrDefaultAsync(c => c.Id == id);

            if (category == null)
            {
                return NotFound();
            }

            // Kontrollera om kategori har nyheter kopplat till sig 
            if (category.Articles != null && category.Articles.Any())
            { // Om ja, förhindra borttagning och skriv ut felmeddelande 
                TempData["ErrorMessage"] = "Du kan inte ta bort en kategori som har nyheter i sig";
                return RedirectToAction(nameof(Delete), new { id = category.Id });
            }

            // annars ta bort kategorin  
            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool CategoryExists(int id)
        {
            // Kontrollera om context är null 
            if (_context.Categories == null)
            {

                return false;
            }

            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
