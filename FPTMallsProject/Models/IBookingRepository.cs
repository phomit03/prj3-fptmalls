using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FPTMallsProject.Models
{
    public interface IBookingRepository
    {
        IEnumerable<Booking> GetAllBookings();
        Booking AddBooking(Booking newBooking);

    }
}
