using FPTMallsProject.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Diagnostics;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace FPTMallsProject.Controllers
{
	public class HomeController : Controller
	{
        private readonly ILogger<HomeController> _logger;
        private readonly AppDbContext _context;

        public HomeController(ILogger<HomeController> logger, AppDbContext context)
        {
            _logger = logger;
            this._context = context;
        }

        public IActionResult Index()
        {
            var fPTMallsContext = _context.BrandIntroduction
                                    .Include(b => b.Brand)
                                    .ThenInclude(s => s.Shop)
                                    .ToList();
            var randomRecords = fPTMallsContext.OrderBy(r => Guid.NewGuid()).Take(6);
            return View(randomRecords);
        }


        //feedback form
        [HttpPost]
        public async Task<IActionResult> Create([Bind("Id,Name,Phone,Email,Title,Description")] Feedback feedback)
        {
            if (ModelState.IsValid)
            {
                _context.Add(feedback);
                await _context.SaveChangesAsync();
                TempData["SuccessMessage"] = "Feedback sent successfully!";
                return RedirectToAction(nameof(Index));
            }
            return View("Index");
        }



        //FPT Cinema
        public IActionResult Movie()
        {
            return View();
        }

        //About
        public IActionResult About()
        {
            return View();
        }

        //Contact
        public IActionResult Contact()
        {
            return View();
        }

        //Disclaimer
        public IActionResult Disclaimer()
        {
            return View();
        }

        //Utilities
        public IActionResult Utilities()
        {
            return View();
        }

        //My FPT
        public IActionResult MyFpt()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
