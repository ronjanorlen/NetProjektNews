using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NetProjektNews.Models;
using NetProjektNews.Data;

namespace NetProjektNews.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly ApplicationDbContext _context; // Databaskontext

    public HomeController(ILogger<HomeController> logger, ApplicationDbContext context)
    {
        _logger = logger;
        _context = context;
    }

    public IActionResult Index()
    {
        return View();
    }

    // Medlems-sida
    public async Task<IActionResult> Privacy()
    {

        // Kontrollera om context är null 
        if (_context.Articles == null) 
        {
            return NotFound();
        }

        // Visa artiklar som ligger i kategori "Medlemmar"
        var articles = await _context.Articles 
        .Include(a => a.Category)
        .Where(a => a.Category != null && a.Category.CategoryName == "Medlemmar")
        .ToListAsync();


        return View(articles);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}
