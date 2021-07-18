using System;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.Entities
{
    public class PreparationTime : IReservation
    {
        public int Id { get; set; }
        public int RentalId { get; private set; }
        public int Unit { get; }
        public DateTime CheckOut => this.Start.AddDays(1);
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }
        public ReservationType Type => ReservationType.PreparationTime;

        private DateTime _startIgnoreTime;

        public PreparationTime(int rentalId, int unit, DateTime startDate)
        {
            this.RentalId = rentalId;
            this.Unit = unit;
            this.Start = startDate;
        }

        public bool OccursOn(DateTime date) => this.Start == date.Date;
    }
}
