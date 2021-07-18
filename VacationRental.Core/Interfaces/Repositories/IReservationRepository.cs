using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IReservationRepository
    {
        Task<Reservation> GetAsync(int id);
        Task<IEnumerable<Reservation>> ListReservationsFromRental(int rentalId);
        Task<int> SaveAsync(Reservation reservation);
        Task SaveAsync(List<Reservation> reservations);
    }
}
