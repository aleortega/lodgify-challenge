using System;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.Entities
{
    public class Booking : IReservation
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; private set; }
        public int Nights { get; set; }

        public DateTime CheckOut => this.Start.AddDays(this.Nights);
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }
        public ReservationType Type => ReservationType.Booking;

        private DateTime _startIgnoreTime;

        public void AssignUnitToOccupy(int unitToOccupy)
        {
            if (unitToOccupy < 1)
                throw new ApplicationException("No unit available for this booking");

            this.Unit = unitToOccupy;
        }

        public bool ConflictsWith(IReservation reservation) => (reservation.Start < this.CheckOut && reservation.CheckOut > this.Start);

        public bool OccursOn(DateTime date) => (date.Date >= this.Start && date.Date < this.CheckOut);
    }
}
