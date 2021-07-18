using System;
using System.Threading.Tasks;
using VacationRental.Application.Extensions;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.ValueObjects.Builders;

namespace VacationRental.Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IReservationRepository _reservationRepository;
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(IReservationRepository reservationRepository, IRentalRepository rentalRepository)
        {
            this._reservationRepository = reservationRepository;
            this._rentalRepository = rentalRepository;
        }

        public async Task<CalendarViewModel> GetCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var rental = await this._rentalRepository.GetAsync(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var rentalBookings = await this._reservationRepository.ListBookingsFromRental(rentalId);

            var calendarModel = CalendarBuilder
                .Get(rental)
                .From(start)
                .For(nights)
                .With(rentalBookings)
                .Build()
                .AsModel();

            return calendarModel;
        }
    }
}
