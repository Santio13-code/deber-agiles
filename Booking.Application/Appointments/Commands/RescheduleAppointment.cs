// Appointments/Commands/RescheduleAppointment.cs
using MediatR;
using Booking.Application.DTOs;
public record RescheduleAppointmentCommand(RescheduleAppointmentRequest Request) : IRequest<AppointmentDto>;
