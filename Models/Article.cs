using System.ComponentModel.DataAnnotations; // Inkludera DataAnnotations
using System.ComponentModel.DataAnnotations.Schema; 

namespace NetProjektNews.Models {
      public class Article {
        // Properties 
        public int Id { get; set; }

        // Nyhetstitel
        [Required(ErrorMessage = "Ange en titel för nyheten")]
        [Display(Name = "Titel")]
        public string? Title { get; set; }

        // Nyhetsinnehåll
        [Required(ErrorMessage = "Ange text till nyheten")]
        [Display(Name = "Innehåll")]
        public string? Content { get; set; }

        // Vem som skapat inlägget 
        [Display(Name = "Skapad av")]
        public string? CreatedBy { get; set; }

        // Datum och tid för när inlägget skapades 
        [Required(ErrorMessage = "Välj ett datum")]
        [Display(Name = "Datum")]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        // Bild 
        [Display(Name = "Bild")]
        public string? ImageName { get; set; } // Lagra i databasen 

        // Filnamn
        [NotMapped]
        [Display(Name = "Bild")]
        public IFormFile? ImageFile { get; set; } // I gränssnittet, ska ej lagras i databasen

        // KategoriID 
        [Display(Name = "Kategori")]
        public int? CategoryId { get; set; }

        // Kategori på nyhet 
        [Display(Name = "Kategori")]
        public Category? Category { get; set; }
    }
}