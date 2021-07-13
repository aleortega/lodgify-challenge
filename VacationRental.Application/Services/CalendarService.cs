using System;
using System.Threading.Tasks;
using VacationRental.Application.Mapper;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;
using VacationRental.Core.Interfaces.Repositories;
using VacationRental.Core.ValueObjects.Builders;

namespace VacationRental.Application.Services
{
    public class CalendarService : ICalendarService
    {
        private readonly IBookingRepository _bookingRepository;
        private readonly IRentalRepository _rentalRepository;

        public CalendarService(IBookingRepository bookingRepository, IRentalRepository rentalRepository)
        {
            this._bookingRepository = bookingRepository;
            this._rentalRepository = rentalRepository;
        }

        public async Task<CalendarViewModel> GetCalendarAsync(int rentalId, DateTime start, int nights)
        {
            var rental = await this._rentalRepository.GetAsync(rentalId);
            if (rental == null)
                throw new ApplicationException("Rental not found");

            var rentalBookings = await this._bookingRepository.ListBookingsFromRental(rentalId);

            var calendar = CalendarBuilder
                .Get(rentalId)
                .From(start)
                .For(nights)
                .With(rentalBookings)
                .Build();

            var calendarModel = ObjectMapper.Mapper.Map<CalendarViewModel>(calendar);

            return calendarModel;
        }
    }
}
