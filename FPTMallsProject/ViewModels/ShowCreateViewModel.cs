using Microsoft.AspNetCore.Mvc.Rendering;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace FPTMallsProject.ViewModels
{
    public class ShowCreateViewModel
    {
        public int MovieId { get; set; }

        [Display(Name ="Movie")]
        public string MovieTitle { get; set; }

        [Required]
        public string Language { get; set; }

        [Display(Name = "Start Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        public DateTime StartDate { get; set; }
        
        [Display(Name ="End Date")]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{dd/MM/yyyy}")]
        [Required(ErrorMessage = "End Date is required")]
        public DateTime EndDate { get; set; }

        [DataType(DataType.Time)]
        [DisplayFormat(DataFormatString = "{HH:mm:ss}")]
        [Required(ErrorMessage = "Time is required")]
        public DateTime Time { get; set; }        

        public IEnumerable<Models.Show> AvailableShowSlot { get; set; }


        [Required(ErrorMessage = "Price is required")]

        public double Price { get; set; }
    }
}
