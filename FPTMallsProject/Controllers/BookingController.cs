using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using FPTMallsProject.Models;
using FPTMallsProject.ViewModels;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Controllers
{
    [Authorize]
    public class BookingController : Controller
    {
        private readonly IMovieRepository _movieRepo;

        private readonly IShowRepository _showRepo;

        private readonly IBookingRepository _bookingRepo;

        private readonly UserManager<IdentityUser> _userManager;

        public BookingController(UserManager<IdentityUser> userManager, IShowRepository showRepo, IMovieRepository movieRepo, IBookingRepository bookingRepo)
        {
            _userManager = userManager;
            _movieRepo = movieRepo;
            _showRepo = showRepo;
            _bookingRepo = bookingRepo;
        }

        [HttpGet]
        public IActionResult Index(int id)
        {
            var movie = _movieRepo.GetMovie(id);

            var showDates = new List<SelectListItem>();
            
            for(int i = 0; i < 5; i++)
            {
                var date = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");
                showDates.Add(new SelectListItem() { Text = date, Value = date, Selected = false });
            }

            var languages = new List<SelectListItem>();

            char[] c = { ',', ' ' };

            foreach (var language in movie.Language.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList())
            {
                languages.Add(new SelectListItem() { Text = language, Value = language, Selected = false });
            }

            BookingIndexViewModel model = new BookingIndexViewModel()
            {
                ShowDates = showDates,
                Languages = languages,
                MovieId = movie.Id,
                MovieTitle = movie.Title
            };
            
            return View(model);
        }

        [HttpPost]
        public IActionResult Index(BookingIndexViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("Shows", model);
            }
            else
            {
                var movie = _movieRepo.GetMovie(model.MovieId);
                var showDates = new List<SelectListItem>();

                for (int i = 0; i < 5; i++)
                {
                    var date = DateTime.Now.AddDays(i).ToString("dd/MM/yyyy");
                    showDates.Add(new SelectListItem() { Text = date, Value = date, Selected = false });
                }

                var languages = new List<SelectListItem>();

                char[] c = { ',', ' ' };

                foreach (var language in movie.Language.Split(c, StringSplitOptions.RemoveEmptyEntries).ToList())
                {
                    languages.Add(new SelectListItem() { Text = language, Value = language, Selected = false });
                }

                model.ShowDates = showDates;
                model.Languages = languages;

                return View(model);
            }
        }

        [HttpGet]
        [ActionName("Shows")]
        public IActionResult SelectShow(BookingIndexViewModel m)
        {
            //get the hours after the current time 30p

            DateTime.TryParseExact(m.ShowDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedShowDate);

            var currentDate = DateTime.Now.Date;    //date now
            var currentDateTime = DateTime.Now.AddMinutes(30);    //time now + 30p'

            var times = _showRepo.GetAllShows()
                .Where(show => show.Movie.Id == m.MovieId &&
                    (parsedShowDate == DateTime.MinValue ||
                        (DateTime.TryParseExact(show.StartDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedStartDate) &&
                            DateTime.TryParseExact(show.EndDate, "dd/MM/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime parsedEndDate) &&
                            parsedStartDate <= parsedShowDate &&
                            parsedEndDate >= parsedShowDate)
                    ) &&
                    show.Language == m.Language &&
                    DateTime.TryParse(show.Time, out DateTime parsedTime) &&
                    (parsedShowDate > currentDate || (parsedShowDate == currentDate && parsedTime >= currentDateTime))) 
                .OrderBy(show => show.Time)     //Sort the list by showtime
                .Select(show => new SelectListItem { Text = show.Time, Value = show.Id.ToString() })
                .ToList();


            return View(new SelectShowViewModel
            {
                Times = times,
                MovieId = m.MovieId,
                MovieTitle = m.MovieTitle,
                ShowDate = m.ShowDate,
                Language = m.Language
            });
            
        }


        [HttpPost]
        [ActionName("Shows")]
        public IActionResult SelectShow(SelectShowViewModel model)
        {
            if (ModelState.IsValid)
            {
                return RedirectToAction("BookTickets", model);
            }
            else
            {
                var times = new List<SelectListItem>();

                var shows = _showRepo.GetAllShows();
                foreach (var show in shows)
                {
                    if (show.Movie.Id == model.MovieId)
                    {
                        if (DateTime.Compare(DateTime.Parse(show.StartDate), DateTime.Parse(model.ShowDate)) <= 0 && DateTime.Compare(DateTime.Parse(show.EndDate), DateTime.Parse(model.ShowDate)) >= 0)
                        {
                            if (show.Language == model.Language)
                            {
                                times.Add(new SelectListItem() { Text = show.Time, Value = show.Id.ToString(), Selected = false });
                            }
                        }
                    }
                }

                model.Times = times;

                return View(model);
            }
        }

        [HttpGet]
        public IActionResult BookTickets(SelectShowViewModel m)
        {
            var show = _showRepo.GetShow(m.ShowId??1);
            ViewBag.MovieTitle = m.MovieTitle;
            ViewBag.ShowDate = m.ShowDate;
            ViewBag.Language = m.Language;
            ViewBag.ShowTime = show.Time;
            ViewBag.ShowId = m.ShowId;

            ViewBag.Price = show.Price;

            var bookedSeats = new List<string>();
            var bookings = _bookingRepo.GetAllBookings();
            foreach(var booking in bookings)
            {
                if(booking.ShowId == m.ShowId && DateTime.Compare(DateTime.Parse(booking.ShowDate),DateTime.Parse(m.ShowDate)) == 0)
                {
                    bookedSeats.Add(booking.SeatNo.ToString());
                }
            }
            var seats = new List<SelectListItem>();
            for(int i = 1; i <= 160; i++)
            {
                if (bookedSeats.Contains(i.ToString()))
                {
                    seats.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = false, Disabled = true });
                }
                else
                {
                    seats.Add(new SelectListItem() { Text = i.ToString(), Value = i.ToString(), Selected = false, Disabled = false });
                }
            }

            return View(seats);
        }

        //Generate code automatic
        private string GenerateCode()
        {
            Random random = new Random();
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            string code = new string(Enumerable.Repeat(chars, 10)
              .Select(s => s[random.Next(s.Length)]).ToArray());
            return "#" + code;
        }

        [HttpPost]
        public async Task<IActionResult> BookTickets(List<SelectListItem> model)
        {
            var user = await _userManager.GetUserAsync(User);
            var show = _showRepo.GetShow(int.Parse(HttpContext.Request.Form["showId"]));

            foreach (var seatNo in model)
            {
                if (seatNo.Selected)
                {

                    var booking = new Booking()
                    {
                        IdentityUser = user,
                        Show = show,
                        ShowDate = HttpContext.Request.Form["showDate"],
                        SeatNo = int.Parse(seatNo.Value),
                        BookingDateTime = DateTime.Now
                    };
                    // generate code
                    string code = GenerateCode();
                    booking.Code = code;

                    _bookingRepo.AddBooking(booking);
                }
            }
            return RedirectToAction("ViewBookings");
        }


        [ActionName("ViewBookings")]
        public async Task<IActionResult> UserBookings()
        {
            var bookings = _bookingRepo.GetAllBookings();
            var user = await _userManager.GetUserAsync(User);

            var userBookings = new List<Booking>();

            foreach(var booking in bookings)
            {
                if(booking.IdentityUser.Id == user.Id)
                {
                    userBookings.Add(booking);
                }
            }

            return View(userBookings);
        }

    }
}
