using System.Collections.Generic;

namespace VacationRental.Core.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units { get; set; }

        public bool IsAvailable(Booking bookingAttempt, IEnumerable<Booking> registeredBookings)
        {
            bool result = true;
            for (var i = 0; i < bookingAttempt.Nights; i++)
            {
                var count = 0;
                foreach (var booking in registeredBookings)
                {
                    if ((booking.Start <= bookingAttempt.Start.Date && booking.CheckOut > bookingAttempt.Start.Date)
                        || (booking.Start < bookingAttempt.CheckOut && booking.CheckOut >= bookingAttempt.CheckOut)
                        || (booking.Start > bookingAttempt.Start && booking.CheckOut < bookingAttempt.CheckOut))
                    {
                        count++;
                    }
                }

                if (count >= this.Units)
                {
                    result = false;
                    break;
                }
            }

            return result;
        }
    }
}
