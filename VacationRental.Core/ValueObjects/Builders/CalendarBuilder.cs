using System;
using System.Collections.Generic;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.ValueObjects.Builders
{
    public static class CalendarBuilder
    {
        public static ICalendarInitializer Get(Rental rental)
        {
            return new Calendar(rental);
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
        ICalendarBuilder With(IEnumerable<IReservation> reservation);
    }

    public interface ICalendarBuilder
    {
        ICalendarBuilt Build();
    }

    public interface ICalendarBuilt
    {
        Rental Rental { get; set; }
        List<CalendarDate> BookingDates { get; set; }
    }
}
