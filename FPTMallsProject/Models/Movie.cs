using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Models
{
    public class Movie
    {
        public int Id { get; set; }

        public string Title { get; set; }

        public string ReleaseDate { get; set; }

        public string Ratings { get; set; }

        public string Duration { get; set; }

        public string Language { get; set; }

        public string Actor { get; set; }

        public string Director { get; set; }

        public string Genre { get; set; }

        public string? Trailer { get; set; }

        public string PosterPath { get; set; }

        [MaxLength, Column(TypeName = "ntext")]
        public string Describe { get; set; }
    }
}
