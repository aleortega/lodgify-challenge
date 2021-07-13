using System;
using System.Threading.Tasks;
using VacationRental.Application.Mapper;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Application.Services
{
    public class BookingService : IBookingService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public BookingService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            this._bookingRepository = bookingRepository;
            this._rentalRepository = rentalRepository;
        }

        public async Task<BookingViewModel> GetBookingAsync(int bookingId)
        {
            var booking = await this._bookingRepository.GetAsync(bookingId);
            var bookingMapped = ObjectMapper.Mapper.Map<BookingViewModel>(booking);
            return bookingMapped;
        }

        public async Task<ResourceIdViewModel> SaveBookingAsync(BookingBindingModel bookingAttemptModel)
        {
            var rentalToBeBooked = await this._rentalRepository.GetAsync(bookingAttemptModel.RentalId);
            if (rentalToBeBooked == null) 
                throw new ApplicationException("Rental not found");

            var registeredBookings = await this._bookingRepository.ListBookingsFromRental(bookingAttemptModel.RentalId);
            var bookingAttempt = ObjectMapper.Mapper.Map<Booking>(bookingAttemptModel);

            if (!rentalToBeBooked.IsAvailable(bookingAttempt, registeredBookings))
                throw new ApplicationException("Not available");

            var result = await this._bookingRepository.SaveAsync(bookingAttempt);
            return new ResourceIdViewModel() { Id = result };
        }
    }
}
