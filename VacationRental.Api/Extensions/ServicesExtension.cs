using Microsoft.Extensions.DependencyInjection;
using VacationRental.Core.Interfaces.Repositories;

namespace VacationRental.Api.Extensions
{
    public static class ServicesExtension
    {
        /// <summary>
        /// I did not take in mind about thread safe since it was not requested in the assessment
        /// </summary>
        public static IServiceCollection UseInMemoryPersistanceLayer(this IServiceCollection services)
        {
            services.AddSingleton<IRentalRepository, Persistance.InMemory.RentalRepository>();
            services.AddSingleton<IReservationRepository, Persistance.InMemory.ReservationRepository>();
            return services;
        }
    }
}
