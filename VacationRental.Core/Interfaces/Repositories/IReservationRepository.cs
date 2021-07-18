using System.Collections.Generic;
using System.Threading.Tasks;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<IReservation> GetAsync(int id);
        Task<IEnumerable<IReservation>> ListBookingsFromRental(int rentalId);
        Task<int> SaveAsync(IReservation reservation);
        Task SaveAsync(List<IReservation> reservations);
    }
}
