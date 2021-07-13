using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using VacationRental.Application.Models;
using VacationRental.Application.Services.Interfaces;

namespace VacationRental.Api.Controllers
{
    [Route("api/v1/rentals")]
    [ApiController]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _service;

        public RentalsController(IRentalService service)
        {
            this._service = service;
        }

        [HttpGet]
        [Route("{rentalId:int}")]
        public async Task<IActionResult> Get(int rentalId)
        {
            var rental = await this._service.GetRentalAsync(rentalId);
            return Ok(rental);
        }

        [HttpPost]
        public async Task<IActionResult> Post(RentalBindingModel model)
        {
            var rentalAddedId = await this._service.SaveRentalAsync(model);
            return Ok(rentalAddedId);
        }
    }
}
