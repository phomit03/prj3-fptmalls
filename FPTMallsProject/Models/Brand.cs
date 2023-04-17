using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace FPTMallsProject.Models
{
    public class Brand
    {
        [Key]
        public int Id { get; set; }

        [Column(TypeName = "nvarchar(250)")]
        [DisplayName("Image")]
        public string? Image { get; set; }

        [Required]
        [NotMapped]
        [DisplayName("Upload File")]
        public IFormFile? ImageFile { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }

        [Required, StringLength(50)]
        public string Level_brand { get; set; }

        [Required]
        public int Id_Shop { get; set; }

        [ForeignKey("Id_Shop")]
        public virtual Shop Shop { get; set; }

    }
}
