namespace Booking.Blazor.Models;

public record AppointmentHistoryVm(
    Guid Id,
    DateTime StartUtc,
    byte Status,
    string? Notes,
    Guid ClientId,
    string ClientName,
    Guid ServiceId,
    string ServiceName
);

public record PagedResult<T>(int Total, int Page, int Size, List<T> Data);
