using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Core.Interfaces;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Persistance.InMemory
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IDictionary<int, IReservation> _reservations;

        public ReservationRepository(IDictionary<int, IReservation> reservations)
        {
            this._reservations = reservations;
        }

        public async Task<IReservation> GetAsync(int id) => 
            await Task.Run(() => this._reservations[id]);

        public async Task<IEnumerable<IReservation>> ListBookingsFromRental(int rentalId) =>
            await Task.Run(() => this._reservations.Values.Where(reservation => reservation.RentalId == rentalId));

        private int GetNewId() => this._reservations.Keys.Count + 1;

        public async Task<int> SaveAsync(IReservation reservation)
        {
            reservation.Id = this.GetNewId();
            this._reservations.Add(reservation.Id, reservation);

            return await Task.Run(() => reservation.Id);
        }

        public async Task SaveAsync(List<IReservation> reservations)
        {
            await Task.Run(() => reservations.ForEach(reservation =>
            {
                reservation.Id = this.GetNewId();
                this._reservations.Add(reservation.Id, reservation);
            }));
        }
    }
}
