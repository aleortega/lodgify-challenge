using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Persistance.InMemory
{
    public class ReservationRepository : IReservationRepository
    {
        private IDictionary<int, Reservation> _reservations { get; set; }

        public ReservationRepository()
        {
            this._reservations = new Dictionary<int, Reservation>();
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

        public async Task<int[]> SaveAsync(List<Reservation> reservations)
        {
            List<Task<int>> saveOperations = new List<Task<int>>();
            foreach (var reservation in reservations)
            {
                var saveOperation = this.SaveAsync(reservation);
                saveOperations.Add(saveOperation);
            }
            return await Task.WhenAll(saveOperations);
        }

        public async Task<int[]> ReplaceRentalReservations(int rentalId, List<Reservation> newReservations)
        {
            var dictWithUpdatedReservations = this._reservations
                .Where(keyValuePair => keyValuePair.Value.RentalId != rentalId)
                .ToDictionary(keyValuePair => keyValuePair.Key, keyValuePair => keyValuePair.Value);
            this._reservations = dictWithUpdatedReservations;
            return await this.SaveAsync(newReservations);
        }
    }
}
