using System.ComponentModel.DataAnnotations; // Inkludera DataAnnotations

namespace NetProjektNews.Models {
      public class Category {
        // Properties 
        public int Id { get; set; }

        // Kategorinamn 
        [Required(ErrorMessage = "Du kan inte lägga till en kategori utan namn")]
        [Display(Name = "Kategori")]
        public string? CategoryName { get; set; }

        public List<Article>? Articles { get; set; } // Kan ha flera inlägg 
    }
}