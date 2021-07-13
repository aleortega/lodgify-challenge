using AutoMapper;
using System;
using VacationRental.Application.Models;
using VacationRental.Core.Entities;
using VacationRental.Core.ValueObjects;

namespace VacationRental.Application.Mapper
{
    public static class ObjectMapper
    {
        private static readonly Lazy<IMapper> Lazy = new Lazy<IMapper>(() =>
        {
            var config = new MapperConfiguration(cfg =>
            {
                cfg.ShouldMapProperty = p => p.GetMethod.IsPublic || p.GetMethod.IsAssembly;
                cfg.AddProfile<DataTransferObjectsMapper>();
            });
            var mapper = config.CreateMapper();
            return mapper;
        });
        public static IMapper Mapper => Lazy.Value;
    }

    public class DataTransferObjectsMapper : Profile
    {
        public DataTransferObjectsMapper()
        {
            CreateMap<Rental, RentalViewModel>().ReverseMap();
            CreateMap<Rental, RentalBindingModel>().ReverseMap();
            CreateMap<Booking, BookingViewModel>().ReverseMap();
            CreateMap<Booking, BookingBindingModel>().ReverseMap();

            CreateMap<CalendarBookingViewModel, CalendarBooking>()
                .ForMember(dest => dest.Booking, opt => opt.MapFrom(src => src.Id))
                .ReverseMap();
            CreateMap<CalendarDateViewModel, CalendarDate>()
                .ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                .ReverseMap();
            CreateMap<CalendarViewModel, Calendar>()
                .ForMember(dest => dest.RentalId, opt => opt.MapFrom(src => src.RentalId))
                .ForMember(dest => dest.BookingDates, opt => opt.MapFrom(src => src.Dates))
                .ReverseMap();
        }
    }
}
