using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetProjektNews.Models;

namespace NetProjektNews.Data;

public class ApplicationDbContext : IdentityDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }
      // Tabeller i databas 
    public DbSet<Article> Articles { get; set; } // Nyhetsartiklar 
    public DbSet<Category> Categories { get; set; } // Kategorier
}
