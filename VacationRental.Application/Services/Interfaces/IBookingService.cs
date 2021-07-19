using System.Threading.Tasks;
using VacationRental.Application.Models;

namespace VacationRental.Application.Services.Interfaces
{
    public interface IBookingService
    {
        Task<BookingViewModel> GetBookingAsync(int bookingId);
        Task<ResourceIdViewModel> SaveBookingAsync(BookingBindingModel rentalModel);
    }
}
