using System;
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
        private readonly IReservationRepository _reservationRepository;

        public RentalService(IRentalRepository rentalRepository, IReservationRepository reservationRepository)
        {
            this._rentalRepository = rentalRepository;
            this._reservationRepository = reservationRepository;
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

        public async Task<RentalViewModel> UpdateRentalSchedule(int rentalId, RentalBindingModel newRentalSchedule)
        {
            var rentalToUpdate = await this._rentalRepository.GetAsync(rentalId);
            if (rentalToUpdate == null) throw new ApplicationException("No rental found with the id provided");

            var newSchedulesReservations = rentalToUpdate.AttempToModifySchedule(newRentalSchedule.Units, newRentalSchedule.PreparationTimeInDays);
            await this._reservationRepository.ReplaceRentalReservations(rentalId, newSchedulesReservations);
            var updatedRental = await this._rentalRepository.Update(rentalId, rentalToUpdate);
            return updatedRental.AsModel();
        }
    }
}
