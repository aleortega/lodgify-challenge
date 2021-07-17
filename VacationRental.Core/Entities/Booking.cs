using System;

namespace VacationRental.Core.Entities
{
    public class Booking
    {
        public int Id { get; set; }
        public int RentalId { get; set; }
        public int Unit { get; private set; }
        public DateTime Start
        {
            get => _startIgnoreTime;
            set => _startIgnoreTime = value.Date;
        }

        private DateTime _startIgnoreTime;
        public int Nights { get; set; }

        public DateTime CheckOut => this.Start.AddDays(this.Nights);

        public void AssignUnitToOccupy(int unitToOccupy)
        {
            if (unitToOccupy == 0)
                throw new ApplicationException("No unit available for this booking");

            this.Unit = unitToOccupy;
        }
    }
}
