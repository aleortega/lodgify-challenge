using System;
using System.Threading.Tasks;
using VacationRental.Application.Extensions;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRentalRepository _rentalRepository;

        public BookingService(IReservationRepository reservationRepository, IRentalRepository rentalRepository)
        {
            this._reservationRepository = reservationRepository;
            this._rentalRepository = rentalRepository;
        }

        public async Task<BookingViewModel> GetBookingAsync(int bookingId)
        {
            var booking = await this._reservationRepository.GetAsync(bookingId);
            var bookingMapped = (booking as Booking).AsModel();
            return bookingMapped;
        }

        public async Task<ResourceIdViewModel> SaveBookingAsync(BookingBindingModel bookingAttemptModel)
        {
            var rentalToBeBooked = await this._rentalRepository.GetAsync(bookingAttemptModel.RentalId);
            if (rentalToBeBooked == null) 
                throw new ApplicationException("Rental not found");

            var registeredBookings = await this._reservationRepository.ListReservationsFromRental(bookingAttemptModel.RentalId);
            var bookingAttempt = bookingAttemptModel.AsEntity();

            var unitAvailable = rentalToBeBooked
                .LoadPriorReservations(registeredBookings)
                .SearchForAvailableUnit(bookingAttempt);
            bookingAttempt.AssignUnitToOccupy(unitAvailable);
            var preparationTimes = rentalToBeBooked.CalculatePreparationTimesFrom(bookingAttempt);

            var result = await this._reservationRepository.SaveAsync(bookingAttempt);
            await this._reservationRepository.SaveAsync(preparationTimes);

            return new ResourceIdViewModel() { Id = result };
        }
    }
}
