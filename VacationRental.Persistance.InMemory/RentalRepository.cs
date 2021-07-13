using System.Collections.Generic;
using System.Threading.Tasks;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Persistance.InMemory
{
    /// <summary>
    /// To scalability and beyond:
    /// Even when this class does not require Async operations, I developed it that way so when we want to switch to another
    /// persistance engine (let's say Sql or MongoDB) we could keep using the same interface.
    /// </summary>
    public class RentalRepository : IRentalRepository
    {
        private readonly IDictionary<int, Rental> _rentals;

        public RentalRepository(IDictionary<int, Rental> rentals)
        {
            this._rentals = rentals;
        }

        public async Task<Rental> GetAsync(int id) => await Task.Run(() => this._rentals[id]);

        /// <summary>
        /// Since this id assignment is bussines agnostic (we only need this when working with InMemory data), I don't consider it a business logic operation
        /// that's why I keep this assignment at the Persistance Layer
        /// </summary>
        public async Task<int> SaveAsync(Rental rental)
        {
            rental.Id = this._rentals.Keys.Count + 1;
            this._rentals.Add(rental.Id, rental);

            return await Task.Run(() => rental.Id);
        }
    }
}
