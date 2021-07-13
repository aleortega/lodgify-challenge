using System;
using System.Threading.Tasks;
using VacationRental.Application.Models;

namespace VacationRental.Application.Services.Interfaces
{
    public interface ICalendarService
    {
        Task<CalendarViewModel> GetCalendarAsync(int rentalId, DateTime start, int nights);
    }
}
