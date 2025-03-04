using System.ComponentModel.DataAnnotations; // Inkludera DataAnnotations
using System.ComponentModel.DataAnnotations.Schema; 

namespace NetProjektNews.Models {
      public class Article {
        // Properties 
        public int Id { get; set; }

        // Nyhetstitel
        [Required]
        [Display(Name = "Titel")]
        public string? Title { get; set; }

        // Nyhetsinnehåll
        [Required]
        [Display(Name = "Innehåll")]
        public string? Content { get; set; }

        // Vem som skapat inlägget 
        [Display(Name = "Skapad av")]
        public string? CreatedBy { get; set; }

        // Datum och tid för när inlägget skapades 
        [Display(Name = "Datum")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Bild 
        public string? ImageName { get; set; } // Lagra i databasen 

        // Filnamn
        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; } // I gränssnittet, ska ej lagras i databasen

        // KategoriID 
        public int? CategoryId { get; set; }

        // Kategori på nyhet 
        public Category? Category { get; set; }
    }
}