namespace Booking.Blazor.Models;
public record AppointmentVm(Guid Id, Guid ClientId, Guid ServiceId, DateTime StartUtc, DateTime EndUtc, byte Status, string? Notes);
public record CreateAppointmentVm(Guid ClientId, Guid ServiceId, DateTime StartUtc, string? Notes);
