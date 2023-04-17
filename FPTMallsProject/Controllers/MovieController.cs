using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FPTMallsProject.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Globalization;
using FPTMallsProject.ViewModels;
using Microsoft.EntityFrameworkCore;
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

namespace FPTMallsProject.Controllers
{
    public class MovieController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        public MovieController(IMovieRepository movieRepo)
        {
            this._movieRepo = movieRepo;
        }

        public ViewResult Index(string type)
        {
            var model = new List<Movie>();
            var movies = _movieRepo.GetAllMovies();

			var currentDateTime = DateTime.Now;

			List<Movie> releasedMovies = movies.Where(movies => DateTime.Parse(movies.ReleaseDate) <= currentDateTime).ToList();
			List<Movie> upcomingMovies = movies.Where(movies => DateTime.Parse(movies.ReleaseDate) > currentDateTime).ToList();

			ViewData["NowShowing"] = releasedMovies;
			ViewData["CoomingSoon"] = upcomingMovies;

			return View();
		}

        public async Task<ViewResult> Details(int id)
        {
            Movie model = _movieRepo.GetMovie(id);

            ViewBag.Title = model.Title;
            ViewBag.Released = DateTime.Parse(model.ReleaseDate).ToString("dd/MM/yyyy");
            ViewBag.Duration = DateTime.Parse(model.Duration).ToString("HH:mm");
            ViewBag.Ratings = model.Ratings;
            ViewBag.Language = model.Language;
            ViewBag.Actor = model.Actor;
            ViewBag.Director = model.Director;
            ViewBag.Genre = model.Genre;
            ViewBag.Describe = model.Describe;
            ViewBag.Trailer = model.Trailer;


            var movies = _movieRepo.GetAllMovies();
            char[] c = { ',', ' ' };
            var genres = model.Genre.Split(c);

            var similarMovies = new List<Movie>();

            foreach(var movie in movies)
            {
                if (genres.Intersect<string>(movie.Genre.Split(c, StringSplitOptions.RemoveEmptyEntries)).Count() >= 2 && model.Id != movie.Id)
                {
                    similarMovies.Add(movie);
                }
            }
            ViewBag.SimilarMovies = similarMovies;
            return View(model);
        }

        private async Task<dynamic> FetchMovieDetails(string title, string releaseYear)
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
		public ViewResult AllMovies()
		{
			var model = new List<Movie>();
			var movies = _movieRepo.GetAllMovies();

			return View(movies);
		}


		[HttpGet]
		public async Task<IActionResult> SearchResults(string keyword)
		{
			ViewData["searching"] = keyword;
			var movies = _movieRepo.GetAllMovies().AsQueryable();
			if (!string.IsNullOrEmpty(keyword))
			{
				movies = movies.Where(m => m.Title.Contains(keyword));
			}
			var movieList = await movies.AsNoTracking().ToListAsync();
			return View(movieList);
		}

        public ViewResult TicketPrice()
        {
            return View();
        }

    }
}
