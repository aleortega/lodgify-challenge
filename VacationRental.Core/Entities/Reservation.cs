using System;

namespace VacationRental.Core.Entities
{
    public abstract class Reservation
    {
        public int Id { get; set;  }
        public int RentalId { get; set; }
        public int Unit { get; protected set; }
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }
        protected DateTime _startIgnoreTime;
        public bool ConflictsWith(Reservation reservation) => (reservation.Start < this.CheckOut && reservation.CheckOut > this.Start);
        public abstract DateTime CheckOut { get; }
        public abstract ReservationType Type { get; }
        public abstract bool OccursOn(DateTime date);
    }
}
