// Appointments/Commands/CancelAppointment.cs
using MediatR;
public record CancelAppointmentCommand(Guid Id) : IRequest<bool>;

