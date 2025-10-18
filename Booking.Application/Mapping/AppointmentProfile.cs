// Mapping/AppointmentProfile.cs
using AutoMapper;
using Booking.Application.DTOs;
using Booking.Domain.Entities;

public class AppointmentProfile : Profile
{
    public AppointmentProfile()
    {
        CreateMap<Appointment, AppointmentDto>();
    }
}
