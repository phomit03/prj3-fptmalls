using FPTMallsProject.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Threading.Tasks;
using System;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace FPTMallsProject.Controllers
{
    public class FptMallController : Controller
    {

        private readonly AppDbContext _context;

        public FptMallController(AppDbContext context)
        {
            _context = context;
        }
        public async Task<IActionResult> Detail(int id)
        {
            var fPTMallsContext = await _context.BrandIntroduction
                                            .Where(x => x.Brand.Id == id)
                                            .Include(b => b.Brand)
                                            .ThenInclude(bt => bt.Shop)
                                            .FirstOrDefaultAsync();
            if (fPTMallsContext == null)
            {
                return NotFound();
            }

            return View(fPTMallsContext);
        }

        public async Task<IActionResult> Index(string searchString, string store_category)
        {
            var brands = from b in _context.Brand.Include(b => b.Shop)
                         select b;

            if (!String.IsNullOrEmpty(searchString))
            {
                ViewBag.searchString = searchString;
                brands = brands.Where(b => b.Name.Contains(searchString));
            }
            if (!String.IsNullOrEmpty(store_category))
            {
                ViewBag.store_category = store_category;
                brands = brands.Where(b => b.Shop.Name.Contains(store_category));
            }

            var shops = _context.Shop.ToList();
            ViewBag.shops = shops;

            return View(await brands.ToListAsync());
        }


        public async Task<IActionResult> Fashion()
        {
            var fPTMallsContext = _context.Brand
                .Where(x => x.Shop.Name == "Fashion")
                .Include(b => b.Shop);
            var shops = _context.Shop.ToList();
            ViewBag.shops = shops;
            return View(await fPTMallsContext.ToListAsync());
        }
        public async Task<IActionResult> FoodCourts()
        {
            var fPTMallsContext = _context.Brand
                .Where(x => x.Shop.Name == "Food Courts")
                .Include(b => b.Shop);
            var shops = _context.Shop.ToList();
            ViewBag.shops = shops;
            return View(await fPTMallsContext.ToListAsync());
        }
        public async Task<IActionResult> Galleries()
        {
            var fPTMallsContext = _context.Brand
                .Where(x => x.Shop.Name == "Galleries")
                .Include(b => b.Shop);
            var shops = _context.Shop.ToList();
            ViewBag.shops = shops;
            return View(await fPTMallsContext.ToListAsync());
        }
        public async Task<IActionResult> Other()
        {
            var fPTMallsContext = _context.Brand
                .Where(x => x.Shop.Name == "Other")
                .Include(b => b.Shop);
            var shops = _context.Shop.ToList();
            ViewBag.shops = shops;
            return View(await fPTMallsContext.ToListAsync());
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
