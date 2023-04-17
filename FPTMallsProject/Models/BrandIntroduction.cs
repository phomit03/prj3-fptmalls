using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace FPTMallsProject.Models
{
    public class BrandIntroduction
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required, Column(TypeName = "ntext")]
        public string Content { get; set; }

        [Required]
        public int Id_Brand { get; set; }

        [ForeignKey("Id_Brand")]
        public virtual Brand Brand { get; set; }

    }
}
