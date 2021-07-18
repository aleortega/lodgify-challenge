using System;
using VacationRental.Core.Entities;

namespace VacationRental.Core.Interfaces
{
    public interface IReservation
    {
        int Id { get; set;  }
        int Unit { get; }
        int RentalId { get; }
        DateTime Start { get; set; }
        DateTime CheckOut { get; }
        ReservationType Type { get; }
        bool OccursOn(DateTime date);
    }
}
