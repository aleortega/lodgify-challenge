using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Persistance.InMemory
{
    public class BookingRepository : IBookingRepository
    {
        private readonly IDictionary<int, Booking> _bookings;

        public BookingRepository(IDictionary<int, Booking> bookings)
        {
            this._bookings = bookings;
        }

        public async Task<Booking> GetAsync(int id) => await Task.Run(() => this._bookings[id]);

        public async Task<IEnumerable<Booking>> ListBookingsFromRental(int rentalId) => await Task.Run(() => this._bookings.Values.Where(booking => booking.RentalId == rentalId));

        public async Task<int> SaveAsync(Booking booking)
        {
            booking.Id = this._bookings.Keys.Count + 1;
            this._bookings.Add(booking.Id, booking);

            return await Task.Run(() => booking.Id);
        }
    }
}
