using System.Threading.Tasks;
using VacationRental.Application.Models;

namespace VacationRental.Application.Services.Interfaces
{
    public interface IRentalService
    {
        Task<RentalViewModel> GetRentalAsync(int rentalId);
        Task<ResourceIdViewModel> SaveRentalAsync(RentalBindingModel rentalModel);
        Task<RentalViewModel> UpdateRentalSchedule(int rentalId, RentalBindingModel newRentalSchedule);
    }
}
