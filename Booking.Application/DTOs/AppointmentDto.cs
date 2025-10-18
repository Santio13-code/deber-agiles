// DTOs/AppointmentDto.cs
using Booking.Domain.Enums;

namespace Booking.Application.DTOs;
public record AppointmentDto(Guid Id, Guid ClientId, Guid ServiceId,
    DateTime StartUtc, DateTime EndUtc, AppointmentStatus Status, string? Notes);

public record CreateAppointmentRequest(Guid ClientId, Guid ServiceId, DateTime StartUtc, string? Notes);
public record RescheduleAppointmentRequest(Guid Id, DateTime NewStartUtc);
