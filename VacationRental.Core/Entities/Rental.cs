using System.Collections.Generic;
using VacationRental.Core.Interfaces;

namespace VacationRental.Core.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }
        public int PreparationTimeInDays { get; set; }

        private HashSet<int> GetUnitsToBook()
        {
            HashSet<int> unitsToBook = new HashSet<int>();
            for (var unit = 0; unit < this.Units; unit++)
                unitsToBook.Add(unit + 1);

            return unitsToBook;
        }

        public int SearchForAvailableUnit(Booking bookingAttempt, IEnumerable<IReservation> registeredBookings)
        {
            var availableUnits = this.GetUnitsToBook();
            var currentNight = 1;

            while (availableUnits.Count > 0 && currentNight <= bookingAttempt.Nights)
            {
                foreach (var booking in registeredBookings)
                {
                    if ((booking.Start <= bookingAttempt.Start.Date && booking.CheckOut.AddDays(this.PreparationTimeInDays) > bookingAttempt.Start.Date)
                        || (booking.Start < bookingAttempt.CheckOut && booking.CheckOut.AddDays(this.PreparationTimeInDays) >= bookingAttempt.CheckOut)
                        || (booking.Start > bookingAttempt.Start && booking.CheckOut.AddDays(this.PreparationTimeInDays) < bookingAttempt.CheckOut))
                    {
                        availableUnits.Remove(booking.Unit);
                    }
                }
                currentNight++;
            }

            return availableUnits.Count;
        }

        public List<IReservation> CalculatePreparationTimesFrom(Booking booking)
        {
            var preparationTimes = new List<IReservation>();
            while(preparationTimes.Count < this.PreparationTimeInDays)
            {
                var preparationTimeStartDate = booking.CheckOut;
                preparationTimes.Add(new PreparationTime(this.Id, booking.Unit, preparationTimeStartDate.AddDays(preparationTimes.Count)));
            }

            return preparationTimes;
        }
    }
}
