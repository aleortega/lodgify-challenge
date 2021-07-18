using System;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.Entities
{
    public class Booking : Reservation
    {
        public int Nights { get; set; }

        public override DateTime CheckOut => this.Start.AddDays(this.Nights);

        public override ReservationType Type => ReservationType.Booking;

        public void AssignUnitToOccupy(int unitToOccupy)
        {
            if (unitToOccupy < 1)
                throw new ApplicationException("No unit available for this booking");

            this.Unit = unitToOccupy;
        }

        public override bool OccursOn(DateTime date) => (date.Date >= this.Start && date.Date < this.CheckOut);
    }
}
