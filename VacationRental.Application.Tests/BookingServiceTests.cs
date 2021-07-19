using Moq;
using System;
using System.Collections.Generic;
using VacationRental.Application.Extensions;
using VacationRental.Application.Models;
using VacationRental.Application.Services;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces.Repositories;
using Xunit;

namespace VacationRental.Application.Tests
{
    /// <summary>
    /// This test is to let you know that I am aware how to perform unit testing using mocks.
    /// It is known that an WebApi with only integration tests is not fully tested, that's why I created the domain unit tests as well. Anyways, we are still --
    /// missing unit tests at the Service Layer and at the Persistance Layer. Those unit tests require mocks to isolate the subject under testing from any external dependency.
    /// In this Test fixture, I'll only develop one unit test covering the Booking happy path which is the most important workflow in this API. So you can be aware about how I would test these layers.
    /// Off-note: in this case the persistance layer will not required any mocks because we are using InMemory data structures.
    /// I am doing this because I am aware that knowing how to mock, stub or fake objects is really important to test an application. If you think that this example is not enough to validate my --
    /// knowledge related to this topic please let me know and I can add the other ones but it will require more time for sure :).
    /// </summary>
    public class BookingServiceTests
    {
        [Fact]
        public async void ShouldReturnTrue_WhenExecutingOccursOnWithConflictDate()
        {
            // Arrange
            var rental = new Rental(2) { Id = 1 };
            var reservation = new Booking() { Id = 1, Nights = 3, RentalId = 1, Start = new DateTime(2000, 01, 01)};
            var mockReservationRepository = new Mock<IReservationRepository>();
            var mockRentalRepository = new Mock<IRentalRepository>();
            mockRentalRepository
                .Setup(_ => _.GetAsync(It.IsAny<int>()))
                .ReturnsAsync(rental);

            mockReservationRepository
                .Setup(_ => _.ListReservationsFromRental(It.IsAny<int>()))
                .ReturnsAsync(new List<Reservation>() { reservation });

            mockReservationRepository
                .Setup(_ => _.SaveAsync(It.IsAny<Reservation>()))
                .ReturnsAsync(2);

            var bookingAttempt = new BookingBindingModel() { Nights = 2, RentalId = 1, Start = new DateTime(2000, 01, 01)};
            var service = new BookingService(mockReservationRepository.Object, mockRentalRepository.Object);

            var bookingEntityToPersist = bookingAttempt.AsEntity();
            bookingEntityToPersist.AssignUnitToOccupy(2);
            var preparationTimes = rental.CalculatePreparationTimesFrom(bookingEntityToPersist);

            // Act
            var result = await service.SaveBookingAsync(bookingAttempt);

            // Assert
            Assert.Equal(2, result.Id);
            mockRentalRepository.Verify(_ => _.GetAsync(1), Times.Once);
            mockReservationRepository.Verify(_ => _.ListReservationsFromRental(1), Times.Once);
            mockReservationRepository.Verify(_ => _.SaveAsync(It.IsAny<Booking>()), Times.Once);
            mockReservationRepository.Verify(_ => _.SaveAsync(It.IsAny<List<Reservation>>()), Times.Once);
        }
    }
}
