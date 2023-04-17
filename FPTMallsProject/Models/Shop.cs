using System.ComponentModel.DataAnnotations;

namespace FPTMallsProject.Models
{
    public class Shop
    {
        [Key]
        public int Id { get; set; }

        [Required, StringLength(150)]
        public string Name { get; set; }
    }
}
