using System.Collections.Generic;

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

        public int GetAvailableUnit(Booking bookingAttempt, IEnumerable<Booking> registeredBookings)
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
    }
}
