using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IBookingRepository
    {
        Task<Booking> GetAsync(int id);
        Task<IEnumerable<Booking>> ListBookingsFromRental(int rentalId);
        Task<int> SaveAsync(Booking booking);
    }
}
