// Appointments/Queries/GetAppointmentsByDay.cs
using MediatR;
using Booking.Application.DTOs;
public record GetAppointmentsByDayQuery(DateTime DayUtc) : IRequest<List<AppointmentDto>>;
