// Appointments/Commands/CreateAppointment.cs
using MediatR;
using Booking.Application.DTOs;
public record CreateAppointmentCommand(CreateAppointmentRequest Request) : IRequest<AppointmentDto>;
