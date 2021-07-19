using System;
using System.Collections.Generic;
using System.Linq;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces;
using VacationRental.Core.ValueObjects.Builders;

namespace VacationRental.Core.ValueObjects
{
    public class Calendar: ICalendarInitializer, ICalendarDelimiter, ICalendarPopulator, ICalendarBuilder, ICalendarBuilt
    {
        public Rental Rental { get; set; }
        public List<CalendarDate> BookingDates { get; set; }
        private DateTime _startDate { get; set; }
        private int _nights { get; set; }
        private List<Reservation> _reservations { get; set; }

        internal Calendar(Rental rental)
        {
            this.Rental = rental;
            this.BookingDates = new List<CalendarDate>();
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

        public ICalendarBuilder With(IEnumerable<Reservation> reservations)
        {
            this._reservations = reservations.ToList();
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

                this._reservations.ForEach(reservation =>
                {
                    if (reservation.OccursOn(date.Date))
                        date.AddReservation(reservation);
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
        public List<CalendarPreparationTime> PreparationTimes { get; set; }
        private Dictionary<ReservationType, Action<Reservation>> _reservationAdder { get; }

        public CalendarDate(DateTime date)
        {
            this.Date = date;
            this.Bookings = new List<CalendarBooking>();
            this.PreparationTimes = new List<CalendarPreparationTime>();
            this._reservationAdder = new Dictionary<ReservationType, Action<Reservation>>()
            {
                { ReservationType.Booking, (Reservation reservation) => this.Bookings.Add(new CalendarBooking(reservation.Id, reservation.Unit)) },
                { ReservationType.PreparationTime, (Reservation reservation) => this.PreparationTimes.Add(new CalendarPreparationTime(reservation.Unit)) }
            };
        }

        public void AddReservation(Reservation reservation)
        {
            this._reservationAdder[reservation.Type](reservation);
        }
    }

    public class CalendarBooking
    {
        public int Booking { get; set; }
        public int Unit { get; set; }

        public CalendarBooking(int booking, int unit)
        {
            this.Booking = booking;
            this.Unit = unit;
        }
    }

    public class CalendarPreparationTime
    {
        public int Unit { get; set; }

        public CalendarPreparationTime(int unit)
        {
            this.Unit = unit;
        }
    }
}
