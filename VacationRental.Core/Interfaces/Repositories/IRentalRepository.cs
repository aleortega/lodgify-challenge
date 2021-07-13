using System.Threading.Tasks;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces.Repositories
{
    public interface IRentalRepository
    {
        Task<Rental> GetAsync(int id);

        Task<int> SaveAsync(Rental rental);
    }
}
