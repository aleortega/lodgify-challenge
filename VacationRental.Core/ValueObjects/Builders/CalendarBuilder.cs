using System;
using System.Collections.Generic;
using VacationRental.Core.Entities;

namespace VacationRental.Core.ValueObjects.Builders
{
    public static class CalendarBuilder
    {
        public static ICalendarInitializer Get(int rentalId)
        {
            return new Calendar(rentalId);
        }
    }

    public interface ICalendarInitializer
    {
        ICalendarDelimiter From(DateTime start);
    }

    public interface ICalendarDelimiter
    {
        ICalendarPopulator For(int nights);
    }

    public interface ICalendarPopulator
    {
        ICalendarBuilder With(IEnumerable<Booking> bookings);
    }

    public interface ICalendarBuilder
    {
        ICalendarBuilt Build();
    }

    public interface ICalendarBuilt
    {
        int RentalId { get; set; }
        List<CalendarDate> BookingDates { get; set; }
    }
}
