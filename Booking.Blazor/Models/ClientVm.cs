namespace Booking.Blazor.Models;
public record ClientVm(Guid Id, string Name, string? Email, string? Phone);
public record CreateClientVm(string Name, string? Email, string? Phone);
