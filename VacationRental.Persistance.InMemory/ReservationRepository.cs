using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Persistance.InMemory
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly IDictionary<int, Reservation> _reservations;

        public ReservationRepository(IDictionary<int, Reservation> reservations)
        {
            this._reservations = reservations;
        }

        public async Task<Reservation> GetAsync(int id) => 
            await Task.Run(() => this._reservations[id]);

        public async Task<IEnumerable<Reservation>> ListReservationsFromRental(int rentalId) =>
            await Task.Run(() => this._reservations.Values.Where(reservation => reservation.RentalId == rentalId));

        private int GetNewId() => this._reservations.Keys.Count + 1;

        public async Task<int> SaveAsync(Reservation reservation)
        {
            reservation.Id = this.GetNewId();
            this._reservations.Add(reservation.Id, reservation);

            return await Task.Run(() => reservation.Id);
        }

        public async Task SaveAsync(List<Reservation> reservations)
        {
            await Task.Run(() => reservations.ForEach(reservation =>
            {
                reservation.Id = this.GetNewId();
                this._reservations.Add(reservation.Id, reservation);
            }));
        }
    }
}
