using System;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.Entities
{
    public class PreparationTime : Reservation
    {
        public override DateTime CheckOut => this.Start.AddDays(1);

        public override ReservationType Type => ReservationType.PreparationTime;

        public PreparationTime(int rentalId, int unit, DateTime startDate)
        {
            this.RentalId = rentalId;
            this.Unit = unit;
            this.Start = startDate;
        }

        public override bool OccursOn(DateTime date) => (this.Start == date.Date);
    }
}
