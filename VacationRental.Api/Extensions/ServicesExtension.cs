using Microsoft.Extensions.DependencyInjection;
using System.Collections.Generic;
using VacationRental.Core.Entities;
using VacationRental.Core.Interfaces;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Api.Extensions
{
    public static class ServicesExtension
    {
        public static IServiceCollection UseInMemoryPersistanceLayer(this IServiceCollection services)
        {
            services.AddSingleton<IDictionary<int, Rental>>(new Dictionary<int, Rental>());
            services.AddSingleton<IDictionary<int, Reservation>>(new Dictionary<int, Reservation>());
            services.AddScoped<IRentalRepository, Persistance.InMemory.RentalRepository>();
            services.AddScoped<IReservationRepository, Persistance.InMemory.ReservationRepository>();
            return services;
        }
    }
}
