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

        public int SearchForAvailableUnit(Booking bookingAttempt, IEnumerable<IReservation> registeredReservations)
        {
            var availableUnits = this.GetUnitsToBook();
            var currentNight = 1;

            while (availableUnits.Count > 0 && currentNight <= bookingAttempt.Nights)
            {
                foreach (var reservation in registeredReservations)
                {
                    if (reservation.Start < bookingAttempt.CheckOut && reservation.CheckOut > bookingAttempt.Start)
                        availableUnits.Remove(reservation.Unit);
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
