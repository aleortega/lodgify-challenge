using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/bookings")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly IBookingService _service;
        public BookingsController(IBookingService service)
        {
            this._service = service;
        }

        [HttpGet]
        [Route("{bookingId:int}")]
        public async Task<IActionResult> Get(int bookingId)
        {
            var booking = await this._service.GetBookingAsync(bookingId);
            return Ok(booking);
        }

        [HttpPost]
        public async Task<IActionResult> Post(BookingBindingModel model)
        {
            if (model.Nights <= 0)
                throw new ApplicationException("Nigts must be positive");

            var bookingAddedId = await this._service.SaveBookingAsync(model);
            return Ok(bookingAddedId);
        }
    }
}
