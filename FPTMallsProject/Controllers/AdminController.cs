using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FPTMallsProject.Models;
using FPTMallsProject.ViewModels;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Hosting;

namespace FPTMallsProject.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        private readonly IShowRepository _showRepo;

        private readonly IBookingRepository _bookingRepo;

        private readonly IWebHostEnvironment _env;

        private readonly AppDbContext _context;

        private readonly IWebHostEnvironment _hostEnvironment;


        public AdminController(IShowRepository showRepo,IMovieRepository movieRepo, IBookingRepository bookingRepo, IWebHostEnvironment env, AppDbContext context, IWebHostEnvironment hostEnvironment)
        {
            _movieRepo = movieRepo;
            _showRepo = showRepo;
            _bookingRepo = bookingRepo;
            _env = env;
            _context = context;
            this._hostEnvironment = hostEnvironment;
        }
        public IActionResult Index()
        {
            return View();
        }


        /************************************************************* Shops ************************************************************/

        public async Task<IActionResult> Shops()
        {
            return _context.Shop != null ?
                        View(await _context.Shop.ToListAsync()) :
                        Problem("Entity set 'AppDbContext.Shop'  is null.");
        }
        public IActionResult CreateShop()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateShop([Bind("Id,Name")] Shop shop)
        {
            if (ModelState.IsValid)
            {
                _context.Add(shop);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Shops));
            }
            return View(shop);
        }

        public async Task<IActionResult> EditShop(int? id)
        {
            if (id == null || _context.Shop == null)
            {
                return NotFound();
            }

            var shop = await _context.Shop.FindAsync(id);
            if (shop == null)
            {
                return NotFound();
            }
            return View(shop);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditShop(int id, [Bind("Id,Name")] Shop shop)
        {
            if (id != shop.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(shop);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ShopExists(shop.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Shops));
            }
            return View(shop);
        }

        public async Task<IActionResult> DeleteShop(int id)
        {
            if (_context.Shop == null)
            {
                return Problem("Entity set 'FPTMallsContext.Shop'  is null.");
            }

            var shop = await _context.Shop.FirstOrDefaultAsync(m => m.Id == id);
            if (shop == null)
            {
                return NotFound();
            }

            _context.Shop.Remove(shop);
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Shops));
        }

        private bool ShopExists(int id)
        {
            return (_context.Shop?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /******************************************************************* Brands *******************************************************************/

        public async Task<IActionResult> Brands()
        {
            var fPTMallsContext = _context.Brand.Include(b => b.Shop);
            return View(await fPTMallsContext.ToListAsync());
        }


        // GET: Brands/Create
        public IActionResult CreateBrand()
        {
            ViewData["Id_Shop"] = new SelectList(_context.Shop, "Id", "Name");
            return View();
        }

        // POST: Brands/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrand([Bind("Id,ImageFile,Name,Level_brand,Id_Shop")] Brand brand)
        {
            if (ModelState.ErrorCount <= 1)
            {
                string wwwRootPath = _hostEnvironment.WebRootPath;
                string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                string extension = Path.GetExtension(brand.ImageFile.FileName);
                brand.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                string path = Path.Combine(wwwRootPath + "/image/", fileName);
                using (var fileStream = new FileStream(path, FileMode.Create))
                {
                    await brand.ImageFile.CopyToAsync(fileStream);
                }

                _context.Add(brand);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Brands));
            }
            ViewData["Id_Shop"] = new SelectList(_context.Shop, "Id", "Name", brand.Id_Shop);
            return View(brand);
        }

        // GET: Brands/Edit/5
        public async Task<IActionResult> EditBrand(int? id)
        {
            if (id == null || _context.Brand == null)
            {
                return NotFound();
            }

            var brand = await _context.Brand.FindAsync(id);
            if (brand == null)
            {
                return NotFound();
            }
            ViewData["Id_Shop"] = new SelectList(_context.Shop, "Id", "Name", brand.Id_Shop);
            return View(brand);
        }

        // POST: Brands/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrand(int id, [Bind("Id,ImageFile,Name,Level_brand,Id_Shop")] Brand brand)
        {
            if (id != brand.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount <= 1)
            {
                try
                {
                    string wwwRootPath = _hostEnvironment.WebRootPath;
                    string fileName = Path.GetFileNameWithoutExtension(brand.ImageFile.FileName);
                    string extension = Path.GetExtension(brand.ImageFile.FileName);
                    brand.Image = fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
                    string path = Path.Combine(wwwRootPath + "/image/", fileName);
                    using (var fileStream = new FileStream(path, FileMode.Create))
                    {
                        await brand.ImageFile.CopyToAsync(fileStream);
                    }

                    _context.Update(brand);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandExists(brand.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Brands));
            }
            ViewData["Id_Shop"] = new SelectList(_context.Shop, "Id", "Name", brand.Id_Shop);
            return View(brand);
        }


        // POST: Brands/Delete/5
        public async Task<IActionResult> DeleteBrand(int id)
        {
            if (_context.Brand == null)
            {
                return Problem("Entity set 'AppDbContext.Brand'  is null.");
            }
            var brand = await _context.Brand.FirstOrDefaultAsync(m => m.Id == id);

            var imagePath = Path.Combine(_hostEnvironment.WebRootPath, "image", brand.Image);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
            }

            if (brand == null)
            {
                return NotFound();
            }
            _context.Brand.Remove(brand);

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Brands));
        }

        private bool BrandExists(int id)
        {
            return (_context.Brand?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /************************************************************* Brand Introduction ************************************************************/

        // GET: BrandIntroductions
        public async Task<IActionResult> BrandIntroductions()
        {
            var fPTMallsContext = _context.BrandIntroduction.Include(bi => bi.Brand);
            return View(await fPTMallsContext.ToListAsync());
        }

        [HttpGet]
        public IActionResult UploadExplorer()
        {
            var dir = new DirectoryInfo(Path.Combine(Directory.GetCurrentDirectory(), _hostEnvironment.WebRootPath, "image_detail"));
            ViewBag.fileInfo = dir.GetFiles();
            return View("FileExplorer");
        }

        [HttpPost]
        public IActionResult UploadImage(IFormFile upload)
        {
            if (upload != null && upload.Length > 0)
            {
                var fileName = Path.GetFileName(upload.FileName);
                var fileExtension = Path.GetExtension(fileName);

                if (fileExtension.ToLower() == ".jpg" || fileExtension.ToLower() == ".jpeg" || fileExtension.ToLower() == ".png" || fileExtension.ToLower() == ".gif" || fileExtension.ToLower() == ".webp")
                {
                    var imagesPath = Path.Combine(_hostEnvironment.WebRootPath, "image_detail");
                    var filePath = Path.Combine(imagesPath, fileName);

                    using (var stream = new FileStream(filePath, FileMode.Create))
                    {
                        upload.CopyTo(stream);
                    }

                    var url = Url.Content("~/image_detail/" + fileName);

                    return Json(new { uploaded = true, url });
                }
            }

            return Json(new { uploaded = false, error = new { message = "Upload failed" } });
        }

        // GET: BrandIntroductions/Create
        public IActionResult CreateBrandIntroduction()
        {
            ViewData["Id_Brand"] = new SelectList(_context.Brand, "Id", "Name");
            return View();
        }

        // POST: BrandIntroductions/Create

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateBrandIntroduction([Bind("Id,Title,Content,Id_Brand")] BrandIntroduction brandIntroduction)
        {
            if (ModelState.ErrorCount <= 1)
            {
                _context.Add(brandIntroduction);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(BrandIntroductions));
            }
            ViewData["Id_Brand"] = new SelectList(_context.Brand, "Id", "Name", brandIntroduction.Id_Brand);
            return View(brandIntroduction);
        }

        // GET: BrandIntroductions/Edit/5
        public async Task<IActionResult> EditBrandIntroduction(int? id)
        {
            if (id == null || _context.BrandIntroduction == null)
            {
                return NotFound();
            }

            var brandIntroduction = await _context.BrandIntroduction.FindAsync(id);
            if (brandIntroduction == null)
            {
                return NotFound();
            }
            ViewData["Id_Brand"] = new SelectList(_context.Brand, "Id", "Name", brandIntroduction.Id_Brand);
            return View(brandIntroduction);
        }

        // Update
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditBrandIntroduction(int id, [Bind("Id,Title,Content,Id_Brand")] BrandIntroduction brandIntroduction)
        {
            if (id != brandIntroduction.Id)
            {
                return NotFound();
            }

            if (ModelState.ErrorCount <= 1)
            {
                try
                {
                    _context.Update(brandIntroduction);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!BrandIntroductionExists(brandIntroduction.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(BrandIntroductions));
            }
            ViewData["Id_Brand"] = new SelectList(_context.Brand, "Id", "Name", brandIntroduction.Id_Brand);
            return View(brandIntroduction);
        }


        // POST: BrandIntroductions/Delete/5
        public async Task<IActionResult> DeleteBrandIntroduction(int id)
        {
            if (_context.BrandIntroduction == null)
            {
                return Problem("Entity set 'AppDbContext.BrandIntroduction'  is null.");
            }
            var brandIntroduction = await _context.BrandIntroduction.FirstOrDefaultAsync(m => m.Id == id);
            if (brandIntroduction == null)
            {
                return NotFound();
            }
            _context.BrandIntroduction.Remove(brandIntroduction);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(BrandIntroductions));
        }

    
        private bool BrandIntroductionExists(int id)
        {
            return (_context.BrandIntroduction?.Any(e => e.Id == id)).GetValueOrDefault();
        }


        /************************************************************* Feedback Customer ************************************************************/

        // GET list Feedbacks
        public async Task<IActionResult> Feedbacks()
        {
            return _context.Feedback != null ?
            View(await _context.Feedback.ToListAsync()) :
            Problem("Entity set 'AppDbContext.Feedback'  is null.");
        }


        /****************************************************************** Movies *********************************************************************/

        public IActionResult Movies()
        {
            var model = _movieRepo.GetAllMovies();
            return View(model);
        }

        [HttpGet]
        public ViewResult CreateMovie()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> CreateMovie(MovieCreateViewModel model)
        {
            if (ModelState.IsValid)
            {
                string path = null;
                if (model.Poster != null)
                    path = ProcessUploadedFile(model.Poster);

                //dynamic movieDetails = await FetchMovieDetails(model.Title,model.ReleaseYear);

                Movie newMovie = new Movie()
                {
                    Title = model.Title,
                    ReleaseDate = model.ReleaseYear,
                    Ratings = model.Ratings.ToString(),
                    Duration = model.Duration.ToString(),
                    Language = model.Language,
                    Actor = model.Actor,
                    Director = model.Director,
                    Genre = model.Genre,
                    Describe = model.Describe,
                    Trailer = model.Trailer,
                    PosterPath = path
                };
                _movieRepo.AddMovie(newMovie);
                return RedirectToAction("Movies");
            }

            return View(model);
        }

        [HttpGet]
        public ViewResult EditMovie(int id)
        {
            var movie = _movieRepo.GetMovie(id);

            MovieEditViewModel model = new MovieEditViewModel()
            {
                Id = movie.Id,
                Title = movie.Title,
                ReleaseYear = movie.ReleaseDate,
                Ratings = movie.Ratings.ToString(), 
                Duration = movie.Duration.ToString(), 
                Language = movie.Language,
                Actor = movie.Actor,
                Director = movie.Director,
                Genre = movie.Genre,
                Describe = movie.Describe,  
                Trailer = movie.Trailer,  
                ExistingPosterPath = movie.PosterPath
            };

            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> EditMovie(MovieEditViewModel model)
        {
            if (ModelState.ErrorCount <= 1)
            {
                Movie movie = _movieRepo.GetMovie(model.Id);

                //dynamic movieDetails = await FetchMovieDetails(model.Title, model.ReleaseYear);

                movie.Title = model.Title;
                movie.ReleaseDate = model.ReleaseYear;
                movie.Ratings = model.Ratings.ToString(); 
                movie.Duration = model.Duration.ToString(); 
                movie.Language = model.Language;
                movie.Actor = model.Actor;
                movie.Director = model.Director;
                movie.Genre = model.Genre;
                movie.Describe = model.Describe;
                movie.Trailer = model.Trailer;


                if (model.Poster != null)
                {
                    if (model.ExistingPosterPath != null)
                    {
                        string path = Path.Combine(_env.WebRootPath, "images_cinema", model.ExistingPosterPath);
                        GC.Collect();
                        GC.WaitForPendingFinalizers();
                        System.IO.File.Delete(path);
                    }
                    movie.PosterPath = ProcessUploadedFile(model.Poster);
                }

                _movieRepo.EditMovie(movie);
                return RedirectToAction("Movies");
            }

            return View(model);
        }

        public IActionResult DeleteMovie(int id)
        {
            _movieRepo.DeleteMovie(id);
            return RedirectToAction("Movies");
        }

        private string ProcessUploadedFile(IFormFile Poster)
        {
            string posterPath = Guid.NewGuid().ToString() + "_" + Poster.FileName;
            string uploadTo = Path.Combine(_env.WebRootPath, "images_cinema");

            Poster.CopyTo(new FileStream(Path.Combine(uploadTo, posterPath), FileMode.Create));

            return posterPath;
        }

        private async Task<dynamic> FetchMovieDetails(string title ,string releaseYear)
        {
            using (var httpClient = new HttpClient())
            {
                using (var response = await httpClient.GetAsync("http://www.omdbapi.com?apikey=e3d108cb&t=" + title + "&y=" + releaseYear))
                {
                    string apiResponse = await response.Content.ReadAsStringAsync();
                    return JsonConvert.DeserializeObject<dynamic>(apiResponse);
                }
            }
        }

        /******************************************************************* Shows *******************************************************************/

        //getAllShows
        [ActionName("Shows")]
        public IActionResult AllShows()
        {
            var model = _showRepo.GetAllShows();
            return View(model);
        }


        //create show
        [HttpGet]
        public IActionResult CreateShow(int id)
        {
            var movie = _movieRepo.GetMovie(id);
            char[] c = { ',', ' ' };

            //check ReleaseDate > DateTime.Now -> block
            if (DateTime.Parse(movie.ReleaseDate) > DateTime.Now)
            {
                return RedirectToAction("Movies", "Admin", new { id = movie.Id });
            }
            else
            {
                ShowCreateViewModel model = new ShowCreateViewModel()
                {
                    MovieId = movie.Id,
                    MovieTitle = movie.Title,
                    Language = movie.Language,
                };

                return View(model);
            }
            
        }

        [HttpPost]
        public IActionResult CreateShow(ShowCreateViewModel model)
        {
            var allShows = _showRepo.GetAllShows();
            var movie = _movieRepo.GetMovie(model.MovieId);
            char[] c = { ',', ' ' };

            model.MovieTitle = movie.Title;
            model.Language = movie.Language;

            //time
            string timeOnly = model.Time.ToString().Remove(0, 10);  //Remove 10 ki tu(d/m/y) de lay times (fix type html)

            string startDateDateOnly = model.StartDate.ToString().Substring(0, 10); //lay 10 ki tu dau remove times (fix type html)
            string endDateDateOnly = model.EndDate.ToString().Substring(0, 10);
            
            //convert datetime
            DateTime startDate = DateTime.ParseExact(startDateDateOnly, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(endDateDateOnly, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (DateTime.Compare(startDate, DateTime.Now.Date) < 0)
            {
                ModelState.AddModelError("StartDate", "You can't select dates prior to today");
                return View(model);
            }

            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "End date must be valid according to start date");
                return View(model);
            }

            //check duplicate shows
            foreach (var s in allShows)
            {
                DateTime startDateShows, endDateShows;
                if (DateTime.TryParseExact(s.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateShows)
                    && DateTime.TryParseExact(s.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateShows))
                {
                    DateTime newStartDate, newEndDate;
                    if (DateTime.TryParseExact(s.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newStartDate)
                        && DateTime.TryParseExact(s.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newEndDate))
                    {
                        if ((newStartDate >= startDateShows && newStartDate <= endDateShows)
                            || (newEndDate >= startDateShows && newEndDate <= endDateShows)
                            || (newStartDate <= startDateShows && newEndDate >= endDateShows))
                        {
							if (s.Time == model.Time.ToString("HH:mm:ss") && s.MovieId == movie.Id) //check time && movies
							{
								ModelState.AddModelError("Time", "Show is already exist for this time slot");
                                return View(model);
                            }
                        }
                    }
                    else
                    {
                        // handle invalid date format
                    }
                }
            }


            Show show = new Show()
            {
                 MovieId = model.MovieId,
                 StartDate = model.StartDate.ToString("dd/MM/yyyy"),
                 EndDate = model.EndDate.ToString("dd/MM/yyyy"),
                 Language = model.Language,
                 Time = model.Time.ToString("HH:mm:ss"),
                 Price = model.Price
            };
            _showRepo.AddShow(show);
            return RedirectToAction("Shows");
        }

        //Edit Show
        [HttpGet]
        public IActionResult EditShow(int id)
        {
            var show = _showRepo.GetShow(id);

            if (show == null)
            {
                return NotFound();
            }

            var model = new ShowEditViewModel
            {
                Id = show.Id,
                MovieId = show.MovieId,
                MovieTitle = show.Movie.Title,
                Language = show.Language,
                StartDate = DateTime.ParseExact(show.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                EndDate = DateTime.ParseExact(show.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture),
                Time = DateTime.ParseExact(show.Time, "HH:mm:ss", CultureInfo.InvariantCulture),
                Price = show.Price
            };

            return View(model);
        }


        [HttpPost]
        public IActionResult EditShow(ShowEditViewModel model)
        {
            // Lấy Show dựa trên id được cung cấp
            var show = _showRepo.GetShow(model.Id);

            // Kiểm tra nếu không tìm thấy Show, trả về NotFound
            if (show == null)
            {
                return NotFound();
            }

            var allShows = _showRepo.GetAllShows();
            var movie = _movieRepo.GetMovie(model.MovieId);
            char[] c = { ',', ' ' };

            model.MovieTitle = movie.Title;
            model.Language = movie.Language;

            string timeOnly = model.Time.ToString().Remove(0, 10);  

            string startDateDateOnly = model.StartDate.ToString().Substring(0, 10);
            string endDateDateOnly = model.EndDate.ToString().Substring(0, 10);
            //convert datetime
            DateTime startDate = DateTime.ParseExact(startDateDateOnly, "dd/MM/yyyy", CultureInfo.InvariantCulture);
            DateTime endDate = DateTime.ParseExact(endDateDateOnly, "dd/MM/yyyy", CultureInfo.InvariantCulture);

            if (DateTime.Compare(startDate, DateTime.Now.Date) < 0)
            {
                ModelState.AddModelError("StartDate", "You can't select dates prior to today");
                return View(model);
            }

            if (DateTime.Compare(startDate, endDate) >= 0)
            {
                ModelState.AddModelError("EndDate", "End date must be valid according to start date");
                return View(model);
            }

			//check duplicate shows
			foreach (var s in allShows)
			{
				if (s.Id == model.Id) // Skip the current show being updated
				{
					continue;
				}

				DateTime startDateShows, endDateShows;
				if (DateTime.TryParseExact(s.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out startDateShows)
					&& DateTime.TryParseExact(s.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out endDateShows))
				{
					DateTime newStartDate, newEndDate;
					if (DateTime.TryParseExact(model.StartDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newStartDate)
						&& DateTime.TryParseExact(model.EndDate.ToString("dd/MM/yyyy"), "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out newEndDate))
					{
						if ((newStartDate >= startDateShows && newStartDate <= endDateShows)
							|| (newEndDate >= startDateShows && newEndDate <= endDateShows)
							|| (newStartDate <= startDateShows && newEndDate >= endDateShows))
						{
							if (s.Time == model.Time.ToString("HH:mm:ss") && s.MovieId == movie.Id) //check time && movies
							{
								ModelState.AddModelError("Time", "Show is already exist for this time slot");
								return View(model);
							}
						}
					}
					else
					{
						// handle invalid date format
					}
				}
			}


			// Update the show properties
			show.MovieId = model.MovieId;
            show.StartDate = model.StartDate.ToString("dd/MM/yyyy");
            show.EndDate = model.EndDate.ToString("dd/MM/yyyy");
            show.Language = model.Language;
            show.Time = model.Time.ToString("HH:mm:ss");
            show.Price = model.Price;

            // Update the show in the repository
            _showRepo.EditShow(show);

            // Chuyển hướng người dùng đến trang hiển thị danh sách các Show
            return RedirectToAction("Shows");
        }

        //delete show by item
        public IActionResult DeleteShow(int id)
        {
            _showRepo.DeleteShow(id);
            return RedirectToAction("Shows");
        }

        //delete all show expired
        public IActionResult DeleteExpiredShow()
        {
            _showRepo.DeleteExpiredShow();
            return RedirectToAction("Shows");
        }

        /************************************************************ bookings *****************************************************************/

        //get all booking user
        [ActionName("Bookings")]
        public IActionResult AllBookings(string searchCode)
        {
            IEnumerable<Booking> model = _bookingRepo.GetAllBookings();

            if (!string.IsNullOrEmpty(searchCode))
            {
                model = model.Where(booking => booking.Code.Contains(searchCode.Trim()));
            }

            return View(model);
        }

    }
}

