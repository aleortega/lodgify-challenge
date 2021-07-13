using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Entities;
using VacationRental.Core.ValueObjects.Builders;

namespace VacationRental.Core.ValueObjects
{
    public class Calendar: ICalendarInitializer, ICalendarDelimiter, ICalendarPopulator, ICalendarBuilder, ICalendarBuilt
    {
        public int RentalId { get; set; }
        public List<CalendarDate> BookingDates { get; set; }
        private DateTime _startDate { get; set; }
        private int _nights { get; set; }
        private List<Booking> _bookings { get; set; }

        internal Calendar(int rentalId)
        {
            this.RentalId = rentalId;
            BookingDates = new List<CalendarDate>();
        }

        public ICalendarDelimiter From(DateTime start)
        {
            this._startDate = start;
            return this;
        }

        public ICalendarPopulator For(int nights)
        {
            this._nights = nights;
            return this;
        }

        public ICalendarBuilder With(IEnumerable<Booking> bookings)
        {
            this._bookings = bookings.ToList();
            return this;
        }
        
        private void AddDateWithBookings(CalendarDate dateWithBookings)
        {
            this.BookingDates.Add(dateWithBookings);
        }

        public ICalendarBuilt Build()
        {
            for (var currentNight = 0; currentNight < this._nights; currentNight++)
            {
                var date = new CalendarDate(this._startDate.AddDays(currentNight));

                this._bookings.ForEach(booking =>
                {
                    if (booking.Start <= date.Date && booking.CheckOut > date.Date)
                        date.AddBooking(booking.Id);
                });

                this.AddDateWithBookings(date);
            }

            return this;
        }
    }

    public class CalendarDate
    {
        public DateTime Date { get; set; }
        public List<CalendarBooking> Bookings { get; set; }

        public CalendarDate(DateTime date)
        {
            this.Date = date;
            this.Bookings = new List<CalendarBooking>();
        }

        public void AddBooking(int booking)
        {
            this.Bookings.Add(new CalendarBooking(booking));
        }
    }

    public class CalendarBooking
    {
        public int Booking { get; set; }

        public CalendarBooking(int booking)
        {
            this.Booking = booking;
        }
    }
}
