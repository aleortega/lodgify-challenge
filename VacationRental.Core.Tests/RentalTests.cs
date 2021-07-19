using System;
using System.Collections.Generic;
using VacationRental.Core.Entities;
using Xunit;

namespace VacationRental.Core.Tests
{
    public class RentalTests
    {
        [Fact]
        public void ShouldCreateARental_WhenAllValuesAreValid()
        {
            // Arrange & Act
            var createdRental = new Rental(1, 1) { Id = 1 };

            // Assert
            Assert.Equal(1, createdRental.Id);
            Assert.Equal(1, createdRental.Units);
            Assert.Equal(1, createdRental.PreparationTimeInDays);
        }

        [Fact]
        public void ShouldCreateARental_WithoutPreparationTimeInDays()
        {
            // Arrange & Act
            var createdRental = new Rental(1) { Id = 1 };

            // Assert
            Assert.Equal(1, createdRental.Id);
            Assert.Equal(1, createdRental.Units);
            Assert.Null(createdRental.PreparationTimeInDays);
        }

        [Fact]
        public void ShouldCreateARental_With0PreparationTimeInDays()
        {
            // Arrange & Act
            var createdRental = new Rental(1, 0) { Id = 1 };

            // Assert
            Assert.Equal(1, createdRental.Id);
            Assert.Equal(1, createdRental.Units);
            Assert.Equal(0, createdRental.PreparationTimeInDays);
        }

        [Fact]
        public void ShouldFailWhileCreatingRental_WhenUnitsValueIsNotPositive()
        {
            // Act & Assert
            Assert.Throws<ApplicationException>(() => new Rental(0));
        }

        [Fact]
        public void ShouldFailWhileCreatingRental_WhenPreparationTimeInDaysValueIsNegative()
        {
            // Act & Assert
            Assert.Throws<ApplicationException>(() => new Rental(1, -1));
        }

        [Fact]
        public void ShouldFailWhileSearchingForAvailableUnit_WhenLoadPriorReservationsFunctionHasNotBeenCalled()
        {
            // Arrange
            var createdRental = new Rental(1, 0) { Id = 1 };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => createdRental.SearchForAvailableUnit(new Booking()));
        }

        [Fact]
        public void ShouldFailWhileAttemptingToModifySchedule_WhenLoadPriorReservationsFunctionHasNotBeenCalled()
        {
            // Arrange
            var createdRental = new Rental(1, 0) { Id = 1 };

            // Act & Assert
            Assert.Throws<ApplicationException>(() => createdRental.AttempToModifySchedule(2, 1));
        }

        [Fact]
        public void ShouldReturnAvailableUnit_WhenCallingSearchForAvailableUnitWithoutReservationsInPlace()
        {
            // Arrange
            var rental = new Rental(1, 0) { Id = 1 };
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };

            // Act
            var unitId = rental
                .LoadPriorReservations(new List<Reservation>())
                .SearchForAvailableUnit(booking);

            // Assert
            Assert.Equal(1, unitId);
        }

        [Fact]
        public void ShouldReturnAvailableUnit_WhenCallingSearchForAvailableUnitWithoutReservationsConflict()
        {
            // Arrange
            var rental = new Rental(1, 0) { Id = 1 };
            var reservationInPlace = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            reservationInPlace.AssignUnitToOccupy(1);
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };

            // Act
            var unitId = rental
                .LoadPriorReservations(new List<Reservation>() { reservationInPlace })
                .SearchForAvailableUnit(bookingAttempt);

            // Assert
            Assert.Equal(1, unitId);
        }

        [Fact]
        public void ShouldReturnAvailableUnitTakingIntoAccountPreparationTimeInDays_WhenCallingSearchForAvailableUnitWithoutReservationsConflict()
        {
            // Arrange
            var rental = new Rental(1, 1) { Id = 1 };
            var bookingReservationInPlace = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            bookingReservationInPlace.AssignUnitToOccupy(1);
            var preparationTimes = rental.CalculatePreparationTimesFrom(bookingReservationInPlace);
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 04) };
            var reservationsInPlace = new List<Reservation>() { bookingReservationInPlace };
            reservationsInPlace.AddRange(preparationTimes);

            // Act
            var unitId = rental
                .LoadPriorReservations(reservationsInPlace)
                .SearchForAvailableUnit(bookingAttempt);

            // Assert
            Assert.Equal(1, unitId);
        }

        [Fact]
        public void ShouldReturnAvailableUnit_WhenCallingSearchForAvailableUnitWithReservationsConflictButMultipleUnits()
        {
            // Arrange
            var rental = new Rental(2) { Id = 1};
            var bookingReservationInPlace = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            bookingReservationInPlace.AssignUnitToOccupy(2);
            var preparationTimes = rental.CalculatePreparationTimesFrom(bookingReservationInPlace);
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            var reservationsInPlace = new List<Reservation>() { bookingReservationInPlace };
            reservationsInPlace.AddRange(preparationTimes);

            // Act
            var unitId = rental
                .LoadPriorReservations(reservationsInPlace)
                .SearchForAvailableUnit(bookingAttempt);

            // Assert
            Assert.Equal(1, unitId);
        }

        [Fact]
        public void ShouldNotReturnAvailableUnit_WhenCallingSearchForAvailableUnitWithReservationsConflict()
        {
            // Arrange
            var rental = new Rental(1) { Id = 1 };
            var bookingReservationInPlace = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            bookingReservationInPlace.AssignUnitToOccupy(1);
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };

            // Act
            var unitId = rental
                .LoadPriorReservations(new List<Reservation>() { bookingReservationInPlace })
                .SearchForAvailableUnit(bookingAttempt);

            // Assert
            Assert.Equal(0, unitId);
        }

        [Fact]
        public void ShouldNotReturnAvailableUnitTakingIntoAccountPreparationTimeInDays_WhenCallingSearchForAvailableUnitWithReservationsConflict()
        {
            // Arrange
            var rental = new Rental(1, 1) { Id = 1 };
            var bookingReservationInPlace = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            bookingReservationInPlace.AssignUnitToOccupy(1);
            var preparationTimes = rental.CalculatePreparationTimesFrom(bookingReservationInPlace);
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };
            var reservationsInPlace = new List<Reservation>() { bookingReservationInPlace };
            reservationsInPlace.AddRange(preparationTimes);

            // Act
            var unitId = rental
                .LoadPriorReservations(reservationsInPlace)
                .SearchForAvailableUnit(bookingAttempt);

            // Assert
            Assert.Equal(0, unitId);
        }

        [Fact]
        public void ShouldReturnBookingsPreparationTimes_WhenCallingCalculatePreparationTimesFrom()
        {
            // Arrange
            var rental = new Rental(1, 2) { Id = 1 };
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };
            bookingAttempt.AssignUnitToOccupy(1);

            // Act
            var calculatedPreparationTimes = rental.CalculatePreparationTimesFrom(bookingAttempt);

            // Assert
            Assert.Equal(2, calculatedPreparationTimes.Count);
            Assert.Equal(calculatedPreparationTimes[0].Unit, bookingAttempt.Unit);
            Assert.Equal(calculatedPreparationTimes[1].Unit, bookingAttempt.Unit);
        }

        [Fact]
        public void ShouldNotReturnBookingsPreparationTimes_WhenCallingCalculatePreparationTimesFromWithoutPreparationTimeInDaysSet()
        {
            // Arrange
            var rental = new Rental(1) { Id = 1 };
            var bookingAttempt = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };
            bookingAttempt.AssignUnitToOccupy(1);

            // Act
            var calculatedPreparationTimes = rental.CalculatePreparationTimesFrom(bookingAttempt);

            // Assert
            Assert.Empty(calculatedPreparationTimes);
        }

        [Fact]
        public void ShouldModifyRentalSchedule_WhenCallingAttemptToModifyScheduleWithoutAnyReservationInPlace()
        {
            // Arrange
            var rental = new Rental(1) { Id = 1 };
            rental.LoadPriorReservations(new List<Reservation>());

            // Act
            rental.AttempToModifySchedule(2, 1);

            // Assert
            Assert.Equal(2, rental.Units);
            Assert.Equal(1, rental.PreparationTimeInDays);        
        }

        [Fact]
        public void ShouldModifyRentalSchedule_WhenCallingAttemptToModifyScheduleWithNonConflictingReservationsInPlace()
        {
            // Arrange
            var rental = new Rental(1) { Id = 1 };
            var reservation = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };
            reservation.AssignUnitToOccupy(1);
            rental.LoadPriorReservations(new List<Reservation>() { reservation });

            // Act
            rental.AttempToModifySchedule(2, 1);

            // Assert
            Assert.Equal(2, rental.Units);
            Assert.Equal(1, rental.PreparationTimeInDays);
        }

        [Fact]
        public void ShouldNotModifyRentalSchedule_WhenCallingAttemptToModifyScheduleWithConflictingReservationsInPlace()
        {
            // Arrange
            var rental = new Rental(1) { Id = 1 };
            var firstReservation = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            firstReservation.AssignUnitToOccupy(1);
            var secondReservation = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 03) };
            secondReservation.AssignUnitToOccupy(1);
            rental.LoadPriorReservations(new List<Reservation>() { firstReservation, secondReservation });

            // Act & Assert
            Assert.Throws<ApplicationException>(() => rental.AttempToModifySchedule(null, 1));
        }
    }
}
