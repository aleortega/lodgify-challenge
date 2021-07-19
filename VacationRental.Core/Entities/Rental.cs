using System;
using System.Collections.Generic;
using System.Linq;

namespace VacationRental.Core.Entities
{
    public class Rental
    {
        public int Id { get; set; }
        public int Units 
        {
            get => _units;
            set
            {
                if (value < 1)
                    throw new ApplicationException("It is not possible to create a Rental with non positive units value");
                _units = value;
            }
        }
        private int _units;
        public int? PreparationTimeInDays
        {
            get => _preparationTimeInDays;
            set
            {
                if (value < 0 && value != null)
                    throw new ApplicationException("It is not possible to create a Rental with negative preparation time in days value");
                _preparationTimeInDays = value;
            }
        }
        private int? _preparationTimeInDays;
        private IEnumerable<Reservation> _priorReservations;

        public Rental(int units, int? preparationTimeInDays)
        {
            this.Units = units;
            this.PreparationTimeInDays = preparationTimeInDays;
        }

        public Rental(int units)
        {
            this.Units = units;
        }

        public Rental LoadPriorReservations(IEnumerable<Reservation> priorReservations)
        {
            this._priorReservations = priorReservations;
            return this;
        }

        private HashSet<int> GetUnitsToBook()
        {
            HashSet<int> unitsToBook = new HashSet<int>();
            for (var unit = 0; unit < this.Units; unit++)
                unitsToBook.Add(unit + 1);

            return unitsToBook;
        }

        public int SearchForAvailableUnit(Booking bookingAttempt)
        {
            if (this._priorReservations == null)
                throw new ApplicationException("It is needed to load prior rental's reservations before looking for an empty unit");

            var availableUnits = this.GetUnitsToBook();
            var currentNight = 1;

            while (availableUnits.Count > 0 && currentNight <= bookingAttempt.Nights)
            {
                foreach (var reservation in this._priorReservations)
                {
                    if (reservation.ConflictsWith(bookingAttempt))
                        availableUnits.Remove(reservation.Unit);
                }
                currentNight++;
            }

            return availableUnits.Count;
        }

        public List<Reservation> CalculatePreparationTimesFrom(Booking booking)
        {
            var preparationTimes = new List<Reservation>();
            while(preparationTimes.Count < this.PreparationTimeInDays)
            {
                var preparationTimeStartDate = booking.CheckOut;
                preparationTimes.Add(new PreparationTime(this.Id, booking.Unit, preparationTimeStartDate.AddDays(preparationTimes.Count)));
            }

            return preparationTimes;
        }

        public List<Reservation> AttempToModifySchedule(int? units, int? preparationTimeInDays)
        {
            if (this._priorReservations == null)
                throw new ApplicationException("It is needed to load prior rental's reservations before attempting to update rental schedule");
            this.Units = units ?? this.Units;
            this.PreparationTimeInDays = preparationTimeInDays ?? this.PreparationTimeInDays;

            var oldReservations = this._priorReservations;

            var updatedReservations = new List<Reservation>();

            oldReservations
                .Where(reservation => reservation.Type == ReservationType.Booking)
                .ToList()
                .ForEach(bookingReservation =>
                {
                    this._priorReservations = updatedReservations;
                    var reservationAsBooking = (bookingReservation as Booking);
                    var unitToAssign = this.SearchForAvailableUnit(reservationAsBooking);
                    reservationAsBooking.AssignUnitToOccupy(unitToAssign);
                    var preparationTimesReservations = this.CalculatePreparationTimesFrom(reservationAsBooking);

                    updatedReservations.Add(reservationAsBooking);
                    updatedReservations.AddRange(preparationTimesReservations);
                });

            return updatedReservations;
        }
    }
}
