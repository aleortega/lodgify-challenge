using System;
using VacationRental.Core.Entities;
using Xunit;

namespace VacationRental.Core.Tests
{
    public class BookingTests
    {
        [Fact]
        public void ShouldReturnStartDatePlusNights_WhenAccessingCheckoutProperty()
        {
            // Arrange & Act
            var bookingStartDate = new DateTime(2000, 01, 01);
            var booking = new Booking() { Nights = 2, Start = bookingStartDate };

            // Assert
            Assert.Equal(bookingStartDate.AddDays(booking.Nights), booking.CheckOut);
        }

        [Fact]
        public void ShouldReturnBookingType_WhenAccessingType()
        {
            // Arrange & Act
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };

            // Assert
            Assert.Equal(ReservationType.Booking, booking.Type);
        }

        [Fact]
        public void ShouldAssignUnit_WhenUnitIsValid()
        {
            // Arrange
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            var unitToAssign = 1;

            // Act
            booking.AssignUnitToOccupy(unitToAssign);

            // Assert
            Assert.Equal(unitToAssign, booking.Unit);
        }

        [Fact]
        public void ShouldFailWhileAssigningUnit_WhenUnitIsInvalid()
        {
            // Arrange
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };
            var unitToAssign = -1;

            // Act & Assert
            Assert.Throws<ApplicationException>(() => booking.AssignUnitToOccupy(unitToAssign));
        }

        [Fact]
        public void ShouldReturnFalse_WhenExecutingOccursOnWithNonConflictDate()
        {
            // Arrange
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };

            // Act
            var result = booking.OccursOn(new DateTime(2000, 01, 03));

            // Assert
            Assert.False(result);
        }

        [Fact]
        public void ShouldReturnTrue_WhenExecutingOccursOnWithConflictDate()
        {
            // Arrange
            var booking = new Booking() { Nights = 2, Start = new DateTime(2000, 01, 01) };

            // Act
            var result = booking.OccursOn(new DateTime(2000, 01, 02));

            // Assert
            Assert.True(result);
        }

        [Fact]
        public void ShouldFailWhileCreatingBooking_WhenNightsValueIsNotPositive()
        {
            // Act & Assert
            Assert.Throws<ApplicationException>(() => new Booking() { Nights = 0, Start = new DateTime(2000, 01, 01) });
        }
    }
}
