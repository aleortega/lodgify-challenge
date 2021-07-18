using System.Threading.Tasks;
using VacationRental.Application.Extensions;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Application.Services
{
    public class RentalService : IRentalService
    {
        private readonly IRentalRepository _rentalRepository;

        public RentalService(IRentalRepository rentalRepository)
        {
            this._rentalRepository = rentalRepository;
        }

        public async Task<RentalViewModel> GetRentalAsync(int rentalId)
        {
            var rental = await this._rentalRepository.GetAsync(rentalId);
            var rentalMapped = rental.AsModel();
            return rentalMapped;
        }

        public async Task<ResourceIdViewModel> SaveRentalAsync(RentalBindingModel rentalModel)
        {
            var rentalMapped = rentalModel.AsEntity();
            var result = await this._rentalRepository.SaveAsync(rentalMapped);
            return new ResourceIdViewModel() { Id = result };
        }
    }
}
