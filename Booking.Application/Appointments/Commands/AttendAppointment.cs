// Appointments/Commands/AttendAppointment.cs
using MediatR;
public record AttendAppointmentCommand(Guid Id) : IRequest<bool>;
