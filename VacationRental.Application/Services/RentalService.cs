using System.Threading.Tasks;
using VacationRental.Application.Mapper;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Entities;
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
            var rentalMapped = ObjectMapper.Mapper.Map<RentalViewModel>(rental);
            return rentalMapped;
        }

        public async Task<ResourceIdViewModel> SaveRentalAsync(RentalBindingModel rentalModel)
        {
            var rentalMapped = ObjectMapper.Mapper.Map<Rental>(rentalModel);
            var result = await this._rentalRepository.SaveAsync(rentalMapped);
            return new ResourceIdViewModel() { Id = result };
        }
    }
}
