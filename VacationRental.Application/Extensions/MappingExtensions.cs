using VacationRental.Application.Mapper;
using VacationRental.Application.Models;
using VacationRental.Core.Entities;
using VacationRental.Core.ValueObjects.Builders;

namespace VacationRental.Application.Extensions
{
    public static class MappingExtensions
    {
        public static Booking AsEntity(this BookingBindingModel bookingModelToMap) => ObjectMapper.Mapper.Map<Booking>(bookingModelToMap);
        
        public static Rental AsEntity(this RentalBindingModel rentalModelToMap) => ObjectMapper.Mapper.Map<Rental>(rentalModelToMap);
        
        public static CalendarViewModel AsModel(this ICalendarBuilt calendarToMap) => ObjectMapper.Mapper.Map<CalendarViewModel>(calendarToMap);
    
        public static BookingViewModel AsModel(this Booking bookingToMap) => ObjectMapper.Mapper.Map<BookingViewModel>(bookingToMap);
        
        public static RentalViewModel AsModel(this Rental rentalToMap) => ObjectMapper.Mapper.Map<RentalViewModel>(rentalToMap);
    }
}
