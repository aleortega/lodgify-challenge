using System;

namespace VacationRental.Core.Entities
{
    public class Booking : Reservation
    {
        public int Nights
        {
            get => _nights;
            set
            {
                if (value < 1)
                    throw new ApplicationException("It is not possible to create a Booking with non positive nights value");
                _nights = value;
            }
        }
        private int _nights;

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
