using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.ViewModels
{
    public class MovieCreateViewModel
    {
        [Required(ErrorMessage = "Title is required")]
        public string Title { get; set; }

        [Required(ErrorMessage ="Release Year is required")]
        [Display(Name = "Release Year")]
        public string ReleaseYear { get; set; }
        
        [Required(ErrorMessage = "Ratings is required")]
        public string Ratings { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        public string Duration { get; set; }

        [Required(ErrorMessage = "Language is required")]
        public string Language { get; set; }

        [Required(ErrorMessage = "Actor is required")]
        public string Actor { get; set; }

        [Required(ErrorMessage = "Dỉrector is required")]
        public string Director { get; set; }

        [Required(ErrorMessage = "Genre is required")]
        public string Genre { get; set; }

        [Required(ErrorMessage = "Poster is required")]
        public IFormFile Poster { get; set; }

        public string? Trailer { get; set; }


        [Required(ErrorMessage = "Describe is required")]
        [MaxLength, Column(TypeName = "ntext")]
        public string Describe { get; set; }
    }
}
